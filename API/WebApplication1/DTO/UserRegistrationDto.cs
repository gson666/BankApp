using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTO
{
    public class UserRegistrationDto
    {
        public required string FirstName { get; set; } = string.Empty;
        public required string LastName { get; set; } = string.Empty;
        [EmailAddress]
        public required string Email { get; set; } = string.Empty;
        public required string UserName {  get; set; } = string.Empty;
        public required string Password { get; set; } = string.Empty;
        public string ?UserImage {  get; set; } = string.Empty;  
    }
}
