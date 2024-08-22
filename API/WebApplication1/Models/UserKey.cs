namespace WebApplication1.Models
{
    public class UserKey
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        public User User { get; set; }
    }
}
