using System.ComponentModel.DataAnnotations;

namespace TransactionsIngest.Server
{
    public class Transaction
    {
        [Key]
        public string SourceSystem { get; set; }

        [Key]
        public int TransactionId { get; set; }

        public string CardNumber { get; set; }

        public string LocationCode { get; set; }

        public string ProductName { get; set; }

        public decimal Amount { get; set; }

        public DateTime TransactionTime { get; set; }

    }
}
