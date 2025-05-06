using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Models.DTOs;
using API.Services.Interfaces;

namespace API.Controllers
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
            return result.Success ? StatusCode(result.StatusCode ?? 200, result) : StatusCode(result.StatusCode ?? 400, result);
        }

        [HttpPost]
        [Authorize(Policy = "AdministratorOrModerator")]
        public async Task<IActionResult> CreateNation([FromBody] NationCreateDTO nationDto)
        {
            var result = await _nationService.CreateNationAsync(nationDto);
            return result.Success ? StatusCode(result.StatusCode ?? 200, result) : StatusCode(result.StatusCode ?? 400, result);
        }

        [HttpGet("id/{nationId}")]
        public async Task<IActionResult> GetNationById(int nationId)
        {
            var result = await _nationService.GetNationByIdAsync(nationId);
            return result.Success ? StatusCode(result.StatusCode ?? 200, result) : StatusCode(result.StatusCode ?? 400, result);
        }

        [HttpGet("name/{nationName}")]
        public async Task<IActionResult> GetNationByName(string nationName)
        {
            var result = await _nationService.GetNationByNameAsync(nationName);
            return result.Success ? StatusCode(result.StatusCode ?? 200, result) : StatusCode(result.StatusCode ?? 400, result);
        }

        [HttpPatch("id/{nationId}")]
        [Authorize(Policy = "AdministratorOrModerator")]
        public async Task<IActionResult> UpdateNation(int nationId, [FromBody] NationUpdateDTO nationDto)
        {
            var result = await _nationService.UpdateNationAsync(nationId, nationDto);
            return result.Success ? StatusCode(result.StatusCode ?? 200, result) : StatusCode(result.StatusCode ?? 400, result);
        }

        [HttpDelete("id/{nationId}")]
        [Authorize(Policy = "AdministratorOrModerator")]
        public async Task<IActionResult> DeleteNation(int nationId)
        {
            var result = await _nationService.DeleteNationAsync(nationId);
            return result.Success ? StatusCode(result.StatusCode ?? 200, result) : StatusCode(result.StatusCode ?? 400, result);
        }
    }
}
