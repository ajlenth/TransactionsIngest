using Microsoft.AspNetCore.Mvc;

namespace TransactionsIngest.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionsController : ControllerBase
    {
        private static readonly string[] SourceSystems =
        [
            "Store Uptown", "Store Downtown", "Online"
        ];

        [HttpGet(Name = "GetTransactions")]
        public IEnumerable<Transaction> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new Transaction
            {
                SourceSystem = SourceSystems[Random.Shared.Next(SourceSystems.Length)],
                TransactionId = Random.Shared.Next(0, int.MaxValue),
                CardNumber = Random.Shared.Next(0, int.MaxValue).ToString(),
                LocationCode = "loc",
                ProductName = "prod",
                Amount = 123,
                TransactionTime = DateTime.Now.AddDays(index),
            })
            .ToArray();
        }
    }
}
