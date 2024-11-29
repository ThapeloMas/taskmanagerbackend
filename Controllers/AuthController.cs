
using Microsoft.AspNetCore.Mvc;
using TodoApp.DTOs;
using TodoApp.Services;

namespace TodoApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto registerDto)
        {
            var result = await _authService.RegisterUser(registerDto.Email, registerDto.Password);
            
            if (result)
                return Ok(new { message = "Registration successful" });
            
            return BadRequest(new { message = "Email already exists" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
        {
            var token = await _authService.LoginUser(loginDto.Email, loginDto.Password);
            
            if (token == null)
                return Unauthorized(new { message = "Invalid email or password" });
            
            return Ok(new { token });
        }
    }
}