namespace WebApplication1.DTO
{
    public class TransactionDto
    {
        public int TransactionId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string PaymentChannel { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int SenderAccountId { get; set; }
        public int ReceiverAccountId { get; set; }
    }
}
