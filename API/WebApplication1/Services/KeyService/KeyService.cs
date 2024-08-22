
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DB;
using WebApplication1.Models;

namespace WebApplication1.Services.KeyService
{
    public class KeyService : IKeyService
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDb _context;

        public KeyService(UserManager<User> userManager, AppDb context)
        {
            _userManager = userManager;
            _context = context;
        }
        public async Task<string> GenerateKeyAsync(string userId)
        {
            var key = Guid.NewGuid().ToString("N").Substring(0, 16);

            var userKey = new UserKey
            {
                UserId = userId,
                Key = key,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.UserKeys.Add(userKey);
            await _context.SaveChangesAsync();

            return key;
        }
        public async Task InvalidateKeyAsync(string userId, string key)
        {
            var userKey = await _context.UserKeys
                .FirstOrDefaultAsync(k => k.UserId == userId && k.Key == key && k.IsActive);

            if (userKey != null)
            {
                userKey.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }
        public async Task<bool> ValidateKeyAsync(string userId, string key)
        {
            var userKey = await _context.UserKeys
                .FirstOrDefaultAsync(k => k.UserId == userId && k.Key == key && k.IsActive);

            return userKey != null;
        }
    }
}
