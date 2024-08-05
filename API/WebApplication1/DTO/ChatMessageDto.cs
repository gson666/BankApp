namespace WebApplication1.DTO
{
    public class ChatMessageDto
    {
        public string Id { get; set; } = string.Empty;
        public string Sender { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }
}
