using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.Services.KeyService;
using WebApplication1.Services.UserService;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IKeyService _keyService;
        public UserController(IUserService userService, IKeyService keyService)
        {
            _userService = userService;
            _keyService = keyService;
        }

        [HttpGet]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetUsers([FromQuery] bool includeDeleted = false)
        {
            var users = await _userService.GetUsersAsync(includeDeleted);
            return Ok(users);
        }

        [Authorize]
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(string userId, [FromQuery] bool includeDeleted = false)
        {
            var user = await _userService.GetUserByIdAsync(userId, includeDeleted);
            return Ok(user);
        }

        [HttpPost("register")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<ActionResult<UserDto>> Register(UserRegistrationDto userDto)
        {
            var createdUser = await _userService.CreateUserAsync(userDto);
            return CreatedAtAction(nameof(GetUserById), new { userId = createdUser.Id }, createdUser);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserLoginDto userDto)
        {
            var (token,key) = await _userService.AuthenticateAsync(userDto.UserName, userDto.Password);
            
            return Ok(new { Token = token, userName = userDto.UserName, Key = key });
        }

        [HttpPost("assign-role")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> AssignRole(string userId, string role)
        {
            var user = await _userService.GetUserByIdAsync(userId,includeDeleted:true);
            if (user == null)
            {
                return NotFound();
            }
            var userModel = new User { Id = user.Id, UserName = user.UserName };
            await _userService.AssignRoleAsync(userId, role);
            return Ok();
        }

        [HttpPut("deactivate/{userId}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeactivateUser(string userId)
        {
            var user = await _userService.DeactivateUser(userId);
            return Ok(user);
        }
        [HttpPut("activate/{userId}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> ActivateUser(string userId)
        {
            var user = await _userService.ActivateUser(userId);
            return Ok(user);
        }

        //[HttpDelete("{userId}")]
        //[Authorize(Policy = "AdminPolicy")]
        //public async Task<IActionResult> DeleteUser(string userId)
        //{
        //    var user = await _userService.DeleteUser(userId);
        //    return Ok(user);
        //}

        [HttpGet("user-count")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> CountUsers()
        {
            return Ok(await _userService.CountUsers());
        }
        [HttpPut("{userId}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> UpdateUser(string userId, [FromForm] UserDto userDto,[FromForm]IFormFile? profileImage)
        {
            try
            {
                var updatedUser = await _userService.UpdateUserAsync(userId, userDto,profileImage);
                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating user: " + ex.Message);

                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpDelete("{userId}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> PermanentDeleteUser(string userId)
        {
            var user  = await _userService.PermanentDeleteUser(userId);
            return Ok(user);
        }
    }
}
