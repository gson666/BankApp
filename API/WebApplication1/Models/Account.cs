using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Account
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal CurrentBalance { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AvailAbleBalance { get; set; }

        public string Mask { get; set; } = string.Empty;
        public string InstitutionId { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Subtype { get; set; } = string.Empty;
        public string ShareableId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
    }
}
