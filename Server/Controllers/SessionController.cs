using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Models.DTOs;
using Server.Services.Interfaces;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "Administrator")]
    public class SessionController : ControllerBase
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllSessions()
        {
            var result = await _sessionService.GetSessionsAsync();
            return (result.Success) ? Ok(result.Data) : NotFound(new { result.Message });
        }

        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetSession(string id)
        {
            var result = await _sessionService.GetSessionByIdAsync(id);
            return (result.Success) ? Ok(result.Data) : NotFound(new { result.Message });
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetSessionsByUserId(string userId)
        {
            var result = await _sessionService.GetSessionByUserIdAsync(userId);
            return (result.Success) ? Ok(result.Data) : NotFound(new { result.Message });
        }

        [HttpGet("role/{roleId}")]
        public async Task<IActionResult> GetSessionsByRoleId(string roleId)
        {
            var result = await _sessionService.GetSessionByRoleIdAsync(roleId);
            return (result.Success) ? Ok(result.Data) : NotFound(new { result.Message });
        }

        [HttpDelete("id/{id}")]
        public async Task<IActionResult> DeleteSession(string id)
        {
            var result = await _sessionService.DeleteSessionAsync(id);
            return (result.Success) ? Ok(new { result.Message }) : NotFound(new { result.Message });
        }
    }
}
