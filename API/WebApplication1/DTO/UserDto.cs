using WebApplication1.Models;

namespace WebApplication1.DTO
{
    public class UserDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Password { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string? userImage {  get; set; }
        public List<AccountDto> Accounts { get; set; } = new List<AccountDto>();  
    }
}
