using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpGet(Name = "GetTransactions")]
        public async Task<ActionResult<IEnumerable<Transaction>>> Get()
        {
            return Ok(await _ctx.Transactions.ToListAsync());
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
                else
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

                            existingTransaction.AuditLog.Add(new Revision { Id = existingTransaction.AuditLog.Count, Field = diff.MemberPath, From = diff.Value1, To = diff.Value2 });
                        }

                        _ctx.Update(existingTransaction);
                    }

                }

            }

            //TODO - check for revoked transactions here

            //save all above changes
            await _ctx.SaveChangesAsync();

            //return updated list ot transactions
            return Ok(await _ctx.Transactions.ToListAsync());
        }
    }
}
