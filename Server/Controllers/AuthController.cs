using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Models.DTOs;
using Server.Services.Interfaces;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            var result = await _authService.RegisterAsync(registerDTO);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(new { Message = "Registration failed", result.Errors });
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var result = await _authService.LoginAsync(loginDTO);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            return Unauthorized(new { Message = "Invalid credentials" });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return Ok(new { Message = "Logged out successfully" });
        }
    }
}
