namespace WebApplication1.Models
{
    public class ChatMessage
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Sender { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
