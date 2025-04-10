using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Models.DTOs;
using Server.Services.Interfaces;
using System.Security.Claims;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService) => _userService = userService;

        [HttpGet("all")]
        [Authorize(Policy = "Administrator")]
        public async Task<IActionResult> GetAllUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null, [FromQuery] string? role = null)
        {
            var result = await _userService.GetAllUsersAsync(page, pageSize, search, role);
            return result.Success ? Ok(result.Data) : NotFound(new { result.Message });
        }

        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var result = await _userService.GetUserByIdAsync(id);
            return result.Success ? Ok(result.Data) : NotFound(new { result.Message });
        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var result = await _userService.GetUserByEmailAsync(email);
            return result.Success ? Ok(result.Data) : NotFound(new { result.Message });
        }

        [HttpPatch("id/{id}")]
        [Authorize(Policy = "Administrator")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserUpdateDTO userDTO)
        {
            var result = await _userService.UpdateUserAsync(id, userDTO);
            return result.Success ? Ok(result.Data) : BadRequest(new { result.Message, result.Errors });
        }

        [HttpDelete("id/{id}")]
        [Authorize(Policy = "Administrator")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await _userService.DeleteUserAsync(id);
            return result.Success ? Ok(new { result.Message }) : NotFound(new { result.Message });
        }

        [HttpPost("assign-role/id/{id}")]
        [Authorize(Policy = "Administrator")]
        public async Task<IActionResult> AssignRoleToUser(string id, [FromBody] RoleDTO roleDTO)
        {
            if (roleDTO == null || string.IsNullOrEmpty(roleDTO.Id))
                return BadRequest(new { Message = "Role ID is required" });

            var result = await _userService.AssignRoleToUserAsync(id, roleDTO.Id);
            return result.Success ? Ok(new { result.Message }) : BadRequest(new { result.Message, result.Errors });
        }

        [HttpPost("remove-role/id/{id}")]
        [Authorize(Policy = "Administrator")]
        public async Task<IActionResult> RemoveRoleFromUser(string id, [FromBody] RoleDTO roleDTO)
        {
            if (roleDTO == null || string.IsNullOrEmpty(roleDTO.Id))
                return BadRequest(new { Message = "Role ID is required" });

            var result = await _userService.RemoveRoleFromUserAsync(id, roleDTO.Id);
            return result.Success ? Ok(new { result.Message }) : BadRequest(new { result.Message, result.Errors });
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetMyUserDetails()
        {
            var result = await _userService.GetMyUserDetailsAsync();
            return result.Success ? Ok(result.Data) : NotFound(new { result.Message });
        }

        [HttpPatch("change-password/id/{id}")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(string id, [FromBody] UserPasswordChangeDTO dto)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var isAdmin = User.IsInRole("Administrator");

            if (currentUserId != id /*&& !isAdmin*/)
                return Forbid();

            var result = await _userService.ChangePasswordAsync(id, dto);
            return result.Success ? Ok(new { result.Message }) : BadRequest(new { result.Message, result.Errors });
        }
    }
}
