using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using ObjectsComparer;

namespace TransactionsIngest.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly DataContext _ctx;

        public TransactionsController(DataContext ctx)
        {
            _ctx = ctx;
        }

        const string STATUS_REVOKED = "Revoked";
        const string STATUS_PENDING = "Pending";
        const string STATUS_FINALIZED = "Finalized";
        const string STATUS_AUTHORIZED = "Authorized";

        string getStatus(Transaction t)
        {
            if (t.IsRevoked) return STATUS_REVOKED;

            var now = DateTime.UtcNow;
            var diff = (now - t.TransactionTime).TotalHours;

            //transaction time is in future - mark as pending
            if (diff < 0) return STATUS_PENDING;

            //transaction time is in past - if older than 24 hours, mark as finalized
            if (diff > 24) return STATUS_FINALIZED;

            //transaction time is within 24 hours - still able to be modified and revoked
            //mark as authorized
            return STATUS_AUTHORIZED;
        }


        [HttpGet(Name = "GetTransactions")]
        public async Task<ActionResult<IEnumerable<Transaction>>> Get()
        {
            var transactionList = await _ctx.Transactions.Include(t => t.AuditLog).ToListAsync();

            foreach(var transaction in transactionList)
            {
                transaction.Status = getStatus(transaction);
            }

            return Ok(transactionList);
        }

        [Route("/import")]
        [HttpPost(Name = "ImportTransactions")]
        public async Task<ActionResult<IEnumerable<Transaction>>> Post(IEnumerable<Transaction> importedTransactions)
        {
            foreach(var transaction in importedTransactions)
            {
                //check if a transaction with this key already exists
                var existingTransaction = await _ctx.Transactions.FindAsync(transaction.SourceSystem, transaction.TransactionId);

                //no such entry exists - add it to the database
                if(existingTransaction == null)
                {
                    await _ctx.AddAsync(transaction);
                }
                //entry exists - check for changes
                //ignore changes if transaction status is finalized
                else if(getStatus(existingTransaction) != STATUS_FINALIZED)
                {
                    var comparer = new ObjectsComparer.Comparer<Transaction>();
                    bool changesDetected = !comparer.Compare(existingTransaction, transaction, out var differences);

                    //cache differences here to store in audit log later
                    var cachedDifferences = new List<Difference>(differences);

                    //changes detected - modify database entry and record changes in audit log
                    if (changesDetected)
                    {
                        _ctx.Entry(existingTransaction).CurrentValues.SetValues(transaction);

                        foreach (var diff in cachedDifferences)
                        {
                            //ignore changes to the audit log itself to avoid infinite loops
                            if (diff.MemberPath.Contains("AuditLog")) continue;

                            existingTransaction.AuditLog.Add(new Revision { Field = diff.MemberPath, From = diff.Value1, To = diff.Value2 });
                        }

                        _ctx.Update(existingTransaction);
                    }

                }

            }

            var existingTransactions = await _ctx.Transactions.ToListAsync();
            foreach(var existingTransaction in existingTransactions)
            {
                //if this transaction is finalized, skip revoke check
                if (getStatus(existingTransaction) == STATUS_FINALIZED) continue;

                var wasRevoked = existingTransaction.IsRevoked;
                var isRevoked = true; //assume revoked until we verify that this transaction exists in the new import snapshot

                foreach(var importedTransaction in importedTransactions)
                {
                    //found a match - this transaction is not revoked
                    if(importedTransaction.SourceSystem == existingTransaction.SourceSystem && importedTransaction.TransactionId == existingTransaction.TransactionId)
                    {
                        isRevoked = false;
                        break;
                    }
                }

                //revoke status changed - update transaction entry & audit log
                if(isRevoked != wasRevoked)
                {
                    existingTransaction.IsRevoked = isRevoked;
                    existingTransaction.AuditLog.Add(new Revision { Field = "IsRevoked", From = wasRevoked.ToString(), To = isRevoked.ToString()});
                    _ctx.Update(existingTransaction);
                }
            }

            //save all above changes
            await _ctx.SaveChangesAsync();

            //return updated list ot transactions
            return Ok(await _ctx.Transactions.Include(t => t.AuditLog).ToListAsync());
        }
    }
}
