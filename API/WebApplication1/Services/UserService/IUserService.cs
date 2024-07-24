using WebApplication1.Models;
using WebApplication1.DTO;
namespace WebApplication1.Services.UserService
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetUserByIdAsync(string id);
        Task<User> CreateUserAsync(UserRegistrationDto userDto);
        Task<string> AuthenticateAsync(string userName, string password);
        Task AssignRoleAsync(User user, string role);
    }
}
