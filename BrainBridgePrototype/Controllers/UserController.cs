using BrainBridgePrototype.DTOs;
using BrainBridgePrototype.Services;
using Microsoft.AspNetCore.Mvc;

namespace BrainBridgePrototype.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDto userDto, string password)
        {
            await _userService.Register(userDto, password);
            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            var token = await _userService.Login(username, password);
            return Ok(new { Token = token });
        }
    }
}
