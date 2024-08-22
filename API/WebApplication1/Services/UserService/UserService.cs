using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Helpers;
using WebApplication1.Models;
using WebApplication1.DTO;
using WebApplication1.DB;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Diagnostics;
using WebApplication1.Services.KeyService;

namespace WebApplication1.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JwtHandler _jwtHandler;
        private readonly IMapper _mapper;
        private readonly AppDb _context;
        private readonly IKeyService _keyService;
        public UserService(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, JwtHandler jwtHandler, IMapper mapper, AppDb context, IKeyService keyService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _jwtHandler = jwtHandler;
            _mapper = mapper;
            _context = context;
            _keyService = keyService;
        }

        public async Task AssignRoleAsync(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (!await _roleManager.RoleExistsAsync(role))
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(role));
                if (!roleResult.Succeeded)
                {
                    throw new Exception("Failed to create role");
                }
            }

            if (user != null)
            {
                var result = await _userManager.AddToRoleAsync(user, role);
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
        }
        public async Task<(string Token, string Key)> AuthenticateAsync(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                throw new InvalidOperationException("Invalid email or password");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

            if (result.Succeeded)
            {
                var token = await _jwtHandler.GenerateJwtToken(user);
                var key = await _keyService.GenerateKeyAsync(user.Id); // Generate the key here

                return (token, key); // Return both the token and the key
            }

            throw new Exception("Invalid email or password");
        }
        //public async Task<string> AuthenticateAsync(string userName, string password)
        //{
        //    var user = await _userManager.FindByNameAsync(userName);
        //    if (user == null)
        //    {
        //        throw new InvalidOperationException("Invalid email or password");
        //    }
        //    var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

        //    if (result.Succeeded)
        //    {
        //        var token = await _jwtHandler.GenerateJwtToken(user);
        //        return token;
        //    }
        //    throw new Exception("Invalid email or password");
        //}

        public async Task<UserDto> CreateUserAsync(UserRegistrationDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            var result = await _userManager.CreateAsync(user, userDto.Password);
            if (result.Succeeded && user != null)
            {
                await _userManager.AddToRoleAsync(user, "User");
                return _mapper.Map<UserDto>(user);
            }
            throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public async Task<UserDto> GetUserByIdAsync(string userId, bool includeDeleted = false)
        {
            var query =   _context.Users
                         .Include(u => u.Accounts)
                         .ThenInclude(a => a.SenderTransactions) // Include related SenderTransactions
                            .Include(u => u.Accounts)
                        .ThenInclude(a => a.ReceiverTransactions) // Include related ReceiverTransactions
                        .AsQueryable();  // Ensure it's an IQue

            if (!includeDeleted)
            {
                query = query.Where(u => !u.IsDeleted);
            }

            var user = await query.FirstOrDefaultAsync(u => u.Id == userId);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<IEnumerable<UserDto>> GetUsersAsync(bool includeDeleted = false)
        {
            var query = _context.Users.AsQueryable();
            if (!includeDeleted)
            {
                query = query.Where(u => !u.IsDeleted);
            }

            var users = await query.ToListAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        // Only for testing
        public async Task SeedAdminUserAsync()
        {
            var adminEmail = "admin@example.com";
            var adminUser = await _userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new User
                {
                    UserName = "admin",
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "User"
                };

                var result = await _userManager.CreateAsync(adminUser, "Admin@123");

                if (result.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync("Admin"))
                    {
                        await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    }

                    await _userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }

        public async Task<int?> CountUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            int numberOfUsers = users.Count;
            return numberOfUsers;
        }

        public async Task<UserDto> DeactivateUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) throw new Exception("User not found");

            user.IsDeleted = true;
            await _userManager.UpdateAsync(user);

            return _mapper.Map<UserDto>(user);
        }
        public async Task<UserDto> ActivateUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) throw new Exception("User not found");
            user.IsDeleted = false;
            await _userManager.UpdateAsync(user);

            return _mapper.Map<UserDto>(user);
        }
        //unavailable method for now 
        public async Task<UserDto> DeleteUser(string userId)
        {
            var userToDelete = await _userManager.FindByIdAsync(userId);
            if (userToDelete == null) throw new Exception("User not found");

            userToDelete.IsDeleted = true;
            await _userManager.UpdateAsync(userToDelete);

            return _mapper.Map<UserDto>(userToDelete);
        }

        public async Task<UserDto> UpdateUserAsync(string userId, UserDto userDto,IFormFile? profileImage)
        {
            var userToUpdate = await _userManager.FindByIdAsync(userId);
            if (userToUpdate == null) throw new Exception("User not found");

            try
            {
                userToUpdate.UserName = userDto.UserName;
                userToUpdate.FirstName = userDto.FirstName;
                userToUpdate.LastName = userDto.LastName;
                userToUpdate.Email = userDto.Email;

                if (profileImage != null)
                {
                    var uploadsFolder = Path.Combine("wwwhelpers", "Images");
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + profileImage.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await profileImage.CopyToAsync(fileStream);
                    }

                    userToUpdate.userImage = "/Images/" + uniqueFileName; 
                }
                else if (!string.IsNullOrEmpty(userDto.userImage))
                {
                    userToUpdate.userImage = userDto.userImage; 
                }

                var result = await _userManager.UpdateAsync(userToUpdate);
                if (result.Succeeded)
                {
                    return _mapper.Map<UserDto>(userToUpdate);
                }
                else
                {
                    throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new InvalidOperationException("The user was updated by another process. Please reload the user and try again.");
            }
        }
        public async Task<UserDto> PermanentDeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) throw new Exception("N/A");
            await _userManager.DeleteAsync(user);

            return _mapper.Map<UserDto>(user);
        }

    }
}
