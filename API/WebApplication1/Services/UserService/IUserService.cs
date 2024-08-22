using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1.Services.UserService
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetUsersAsync(bool includeDeleted);
        Task<UserDto> GetUserByIdAsync(string id, bool includeDeleted);
        Task<UserDto> CreateUserAsync(UserRegistrationDto userDto);
        //Task<string> AuthenticateAsync(string userName, string password);
        Task<(string Token, string Key)> AuthenticateAsync(string userName, string password);
        Task AssignRoleAsync(string userId, string role);
        Task SeedAdminUserAsync();
        Task<int?> CountUsers();
        Task<UserDto> DeactivateUser(string userId);
        Task<UserDto> ActivateUser(string userId);
        Task<UserDto> DeleteUser(string userId);
        Task<UserDto> UpdateUserAsync(string userId, UserDto user,IFormFile? profileImage);
        Task<UserDto> PermanentDeleteUser(string userId);
        
    }
}
