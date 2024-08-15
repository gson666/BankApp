using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTO
{
    public class UserLoginDto
    {
        [Required(ErrorMessage = "User Name Required")]
        public string UserName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password Required")]
        public string Password { get; set; } = string.Empty;
    }
}
