using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;
        public string? userImage { get; set; }
        public ICollection<Account>? Accounts { get; set; }
    }
}
