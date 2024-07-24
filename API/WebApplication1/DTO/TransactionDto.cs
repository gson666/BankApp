namespace WebApplication1.DTO
{
    public class TransactionDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Date { get; set; } = string.Empty;
        public string PaymentChannel { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string SenderBankId { get; set; } = string.Empty;
        public string ReceiverBankId { get; set; } = string.Empty;
    }
}
