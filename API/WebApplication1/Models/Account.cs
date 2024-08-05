using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Account
    {
        public int AccountId { get; set; }
        public string Name { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal CurrentBalance { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AvailAbleBalance { get; set; }

        public string InstitutionId { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;
        public User User { get; set; }

        public ICollection<Transaction> SenderTransactions { get; set; } = new List<Transaction>();
        public ICollection<Transaction> ReceiverTransactions { get; set; } = new List<Transaction>();
    }
}
