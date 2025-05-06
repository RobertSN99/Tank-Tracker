using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API.Models.DTOs;
using API.Services.Interfaces;
using System.Security.Claims;

namespace API.Controllers
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
            return result.Success ? StatusCode(result.StatusCode ?? 200, result) : StatusCode(result.StatusCode ?? 400, result);
        }

        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var result = await _userService.GetUserByIdAsync(id);
            return result.Success ? StatusCode(result.StatusCode ?? 200, result) : StatusCode(result.StatusCode ?? 400, result);
        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var result = await _userService.GetUserByEmailAsync(email);
            return result.Success ? StatusCode(result.StatusCode ?? 200, result) : StatusCode(result.StatusCode ?? 400, result);                                         
        }

        [HttpGet("name/{userName}")]
        public async Task<IActionResult> GetUserByName(string userName)
        {
            var result = await _userService.GetUserByNameAsync(userName);
            return result.Success ? StatusCode(result.StatusCode ?? 200, result) : StatusCode(result.StatusCode ?? 400, result);
        }

        [HttpPatch("id/{id}")]
        [Authorize(Policy = "Administrator")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserUpdateDTO userDTO)
        {
            var result = await _userService.UpdateUserAsync(id, userDTO);
            return result.Success ? StatusCode(result.StatusCode ?? 200, result) : StatusCode(result.StatusCode ?? 400, result);
        }

        [HttpDelete("id/{id}")]
        [Authorize(Policy = "Administrator")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await _userService.DeleteUserAsync(id);
            return result.Success ? StatusCode(result.StatusCode ?? 200, result) : StatusCode(result.StatusCode ?? 400, result);
        }

        [HttpPost("assign-role/id/{id}")]
        [Authorize(Policy = "Administrator")]
        public async Task<IActionResult> AssignRoleToUser(string id, [FromBody] RoleDTO roleDTO)
        {
            var result = await _userService.AssignRoleToUserAsync(id, roleDTO.Id);
            return result.Success ? StatusCode(result.StatusCode ?? 200, result) : StatusCode(result.StatusCode ?? 400, result);
        }

        [HttpPost("remove-role/id/{id}")]
        [Authorize(Policy = "Administrator")]
        public async Task<IActionResult> RemoveRoleFromUser(string id, [FromBody] RoleDTO roleDTO)
        {
            var result = await _userService.RemoveRoleFromUserAsync(id, roleDTO.Id);
            return result.Success? StatusCode(result.StatusCode ?? 200, result) : StatusCode(result.StatusCode ?? 400, result);
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetMyUserDetails()
        {
            var result = await _userService.GetMyUserDetailsAsync();
            return result.Success ? StatusCode(result.StatusCode ?? 200, result) : StatusCode(result.StatusCode ?? 400, result);
        }

        [HttpPatch("change-password/id/{id}")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(string id, [FromBody] UserPasswordChangeDTO dto)
        {
            var result = await _userService.ChangePasswordAsync(id, dto);
            return result.Success ? StatusCode(result.StatusCode ?? 200, result) : StatusCode(result.StatusCode ?? 400, result);
        }
    }
}
