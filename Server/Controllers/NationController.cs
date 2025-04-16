using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Models.DTOs;
using Server.Services.Interfaces;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NationController : ControllerBase
    {
        private readonly INationService _nationService;

        public NationController(INationService nationService)
        {
            _nationService = nationService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllNations()
        {
            var result = await _nationService.GetAllNationsAsync();
            return result.Success ? Ok(result.Data) : NotFound(new { result.Message });
        }

        [HttpPost]
        [Authorize(Policy = "AdministratorOrModerator")]
        public async Task<IActionResult> CreateNation([FromBody] NationCreateDTO nationDto)
        {
            var result = await _nationService.CreateNationAsync(nationDto);
            return result.Success ? Ok(result.Data) : BadRequest(new { result.Message });
        }

        [HttpGet("id/{nationId}")]
        public async Task<IActionResult> GetNationById(int nationId)
        {
            var result = await _nationService.GetNationByIdAsync(nationId);
            return result.Success ? Ok(result.Data) : NotFound(new { result.Message });
        }

        [HttpPatch("id/{nationId}")]
        [Authorize(Policy = "AdministratorOrModerator")]
        public async Task<IActionResult> UpdateNation(int nationId, [FromBody] NationUpdateDTO nationDto)
        {
            var result = await _nationService.UpdateNationAsync(nationId, nationDto);
            return result.Success ? Ok(result.Data) : BadRequest(new { result.Message });
        }

        [HttpDelete("id/{nationId}")]
        [Authorize(Policy = "AdministratorOrModerator")]
        public async Task<IActionResult> DeleteNation(int nationId)
        {
            var result = await _nationService.DeleteNationAsync(nationId);
            return result.Success ? Ok(result.Data) : NotFound(new { result.Message });
        }
    }
}
