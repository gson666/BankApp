using WebApplication1.Models;

namespace WebApplication1.DTO
{
    public class AccountDto
    {
        public int AccountId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal CurrentBalance { get; set; }
        public decimal AvailableBalance { get; set; }
        public string InstitutionId { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
    }
}
