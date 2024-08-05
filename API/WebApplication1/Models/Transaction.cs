using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public string Name { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public PaymentChannel PaymentChannel { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int SenderAccountId { get; set; }
        public Account SenderAccount { get; set; }

        public int ReceiverAccountId { get; set; }
        public Account ReceiverAccount { get; set; }
    }
}
