namespace WebApplication1.DTO
{
    public class AccountDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal CurrentBalance { get; set; }
        public decimal AvailableBalance { get; set; }
        public string Mask { get; set; } = string.Empty;
        public string InstitutionId { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Subtype { get; set; } = string.Empty;
        public string ShareableId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
    }
}
