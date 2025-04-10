using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Models.DTOs;
using Server.Services.Interfaces;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService adminService)
        {
            _roleService = adminService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllRoles()
        {
            var result = await _roleService.GetAllRolesAsync();
            return result.Success ? Ok(result.Data) : NotFound(new { result.Message });
        }

        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetRoleById(string id)
        {
            var result = await _roleService.GetRoleByIdAsync(id);
            return result.Success ? Ok(result.Data) : NotFound(new { result.Message });
        }

        [HttpPost]
        //[Authorize(Policy = "Administrator")]
        public async Task<IActionResult> CreateRole([FromBody] RoleDTO roleDTO)
        {
            var result = await _roleService.CreateRoleAsync(roleDTO!.Name!);
            return result.Success ? Ok(result.Data) : BadRequest(new { result.Message });
        }

        [HttpPut("id/{id}")]
        //[Authorize(Policy = "Administrator")]
        public async Task<IActionResult> UpdateRole(string id, [FromBody] RoleDTO roleDTO)
        {
            if (string.IsNullOrEmpty(roleDTO.Name))
            {
                return BadRequest("Role name cannot be null or empty.");
            }
            var role = await _roleService.UpdateRoleAsync(id, roleDTO.Name);
            if (role == null)
            {
                return NotFound();
            }
            return Ok(role);
        }

        [HttpDelete("id/{id}")]
        //[Authorize(Policy = "Administrator")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            await _roleService.DeleteRoleAsync(id);
            return NoContent();
        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetRoleByName(string name)
        {
            var role = await _roleService.GetRoleByNameAsync(name);
            if (role == null)
            {
                return NotFound();
            }
            return Ok(role);
        }
    }
}
