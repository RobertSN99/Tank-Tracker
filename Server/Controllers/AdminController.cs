using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Services.Interfaces;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _adminService.GetRolesAsync();
            return Ok(roles);
        }

        [HttpGet("roles/id/{id}")]
        public async Task<IActionResult> GetRoleById(string id)
        {
            var role = await _adminService.GetRoleByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            return Ok(role);
        }

        [HttpPost("roles")]
        public async Task<IActionResult> CreateRole([FromBody] RoleDTO roleDTO)
        {
            if (string.IsNullOrEmpty(roleDTO.Name))
            {
                return BadRequest("Role name cannot be null or empty.");
            }
            var role = await _adminService.CreateRoleAsync(roleDTO.Name);
            return CreatedAtAction(nameof(GetRoleById), new { id = role.Id }, role);
        }

        [HttpPut("roles/{id}")]
        public async Task<IActionResult> UpdateRole(string id, [FromBody] RoleDTO roleDTO)
        {
            if (string.IsNullOrEmpty(roleDTO.Name))
            {
                return BadRequest("Role name cannot be null or empty.");
            }
            var role = await _adminService.UpdateRoleAsync(id, roleDTO.Name);
            if (role == null)
            {
                return NotFound();
            }
            return Ok(role);
        }

        [HttpDelete("roles/{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _adminService.GetRoleByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            await _adminService.DeleteRoleAsync(id);
            return NoContent();
        }

        [HttpGet("roles/name/{name}")]
        public async Task<IActionResult> GetRoleByName(string name)
        {
            var role = await _adminService.GetRoleByNameAsync(name);
            if (role == null)
            {
                return NotFound();
            }
            return Ok(role);
        }
    }

    public class RoleDTO
    {
        public string Name { get; set; }
    }
}
