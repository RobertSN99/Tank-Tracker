using Microsoft.AspNetCore.Authorization;
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
            return result.Success ? StatusCode(result.StatusCode ?? 200, result) : StatusCode(result.StatusCode ?? 400, result);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var result = await _authService.LoginAsync(loginDTO);
            return result.Success? StatusCode(result.StatusCode ?? 200, result) : StatusCode(result.StatusCode ?? 400, result);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var result = await _authService.LogoutAsync();
            return result.Success ? StatusCode(result.StatusCode ?? 200, result) : StatusCode(result.StatusCode ?? 400, result);
        }

        [HttpGet("access-denied")]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return Unauthorized(new { Message = "Access denied" });
        }
    }
}
