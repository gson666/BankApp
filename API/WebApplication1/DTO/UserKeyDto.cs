namespace WebApplication1.DTO
{
    public class UserKeyDto
    {
        public string Key { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
