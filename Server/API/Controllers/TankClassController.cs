using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Models.DTOs;
using API.Services.Interfaces;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TankClassController : ControllerBase
    {
        private readonly ITankClassService _tankClassService;

        public TankClassController(ITankClassService tankClassService)
        {
            _tankClassService = tankClassService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllClasses()
        {
            var result = await _tankClassService.GetAllClassesAsync();
            return result.Success ? StatusCode(result.StatusCode ?? 200, result) : StatusCode(result.StatusCode ?? 400, result);
        }

        [HttpPost]
        [Authorize(Policy = "AdministratorOrModerator")]
        public async Task<IActionResult> CreateClass([FromBody] TankClassCreateDTO tankClassDto)
        {
            var result = await _tankClassService.CreateClassAsync(tankClassDto);
            return result.Success ? StatusCode(result.StatusCode ?? 200, result) : StatusCode(result.StatusCode ?? 400, result);
        }

        [HttpGet("id/{classId}")]
        public async Task<IActionResult> GetClassById(int classId)
        {
            var result = await _tankClassService.GetClassByIdAsync(classId);
            return result.Success ? StatusCode(result.StatusCode ?? 200, result) : StatusCode(result.StatusCode ?? 400, result);
        }

        [HttpPatch("id/{classId}")]
        [Authorize(Policy = "AdministratorOrModerator")]
        public async Task<IActionResult> UpdateClass(int classId, [FromBody] TankClassUpdateDTO tankClassDto)
        {
            var result = await _tankClassService.UpdateClassAsync(classId, tankClassDto);
            return result.Success ? StatusCode(result.StatusCode ?? 200, result) : StatusCode(result.StatusCode ?? 400, result);
        }

        [HttpDelete("id/{classId}")]
        [Authorize(Policy = "AdministratorOrModerator")]
        public async Task<IActionResult> DeleteClass(int classId)
        {
            var result = await _tankClassService.DeleteClassAsync(classId);
            return result.Success ? StatusCode(result.StatusCode ?? 200, result) : StatusCode(result.StatusCode ?? 400, result);
        }
    }
}
