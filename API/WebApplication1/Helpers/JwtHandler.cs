using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Models;

namespace WebApplication1.Helpers
{
    public class JwtHandler
    {
        private readonly JwtSettings _jwtSettings;
        private readonly UserManager<User> _userManager;

        public JwtHandler(IOptions<JwtSettings> settings, UserManager<User> userManager)
        {
            _jwtSettings = settings.Value;
            _userManager = userManager;
        }

        public async Task<string> GenerateJwtToken(User user)
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims(user);
            var tokenOptions = new JwtSecurityToken(
                issuer: _jwtSettings.ValidIssuer,
                audience: _jwtSettings.ValidAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_jwtSettings.ExpirationInMinutes)),
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(_jwtSettings.SecurityKey);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256Signature);
        }

        private async Task<List<Claim>> GetClaims(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };
            var userRoles =await _userManager.GetRolesAsync(user);
            claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            return claims;
        }
    }
}
