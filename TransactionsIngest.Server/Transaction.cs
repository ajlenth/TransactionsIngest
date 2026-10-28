using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TransactionsIngest.Server
{
    [PrimaryKey(nameof(SourceSystem), nameof(TransactionId))]
    public class Transaction
    {
        public string SourceSystem { get; set; }

        public int TransactionId { get; set; }

        public string CardNumber { get; set; }

        public string LocationCode { get; set; }

        public string ProductName { get; set; }

        public decimal Amount { get; set; }

        public DateTime TransactionTime { get; set; }

    }
}
