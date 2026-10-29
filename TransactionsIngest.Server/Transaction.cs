using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;
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

        public List<Revision> AuditLog { get; set; } = [];

    }

    [ComplexType]
    public class Revision
    {
        public int Id { get; set; }
        public string Field { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }

}
