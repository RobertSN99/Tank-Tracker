using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Models.DTOs;
using API.Services.Interfaces;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TankController : ControllerBase
    {
        private readonly ITankService _tankService;

        public TankController(ITankService tankService)
        {
            _tankService = tankService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllTanks(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 6,
            [FromQuery] List<string>? nationNames = null,
            [FromQuery] List<string>? statusNames = null,
            [FromQuery] List<string>? tankClassNames = null,
            [FromQuery] List<int>? tiers = null,
            [FromQuery] List<double>? ratings = null,
            [FromQuery] string? searchTerm = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] string? sortOrder = "asc")
        {
            const int maxPageSize = 50;
            if (pageSize > maxPageSize)
                return BadRequest($"Page size cannot exceed {maxPageSize}.");

            var result = await _tankService.GetAllTanksAsync(pageNumber, pageSize, nationNames, statusNames, tankClassNames, tiers, ratings, searchTerm, sortBy, sortOrder);
            return result.Success ? StatusCode(result.StatusCode ?? 200, result) : StatusCode(result.StatusCode ?? 400, result);
        }

        [HttpGet("id/{tankId}")]
        public async Task<IActionResult> GetTankById(int tankId)
        {
            var result = await _tankService.GetTankByIdAsync(tankId);
            return result.Success ? StatusCode(result.StatusCode ?? 200, result) : StatusCode(result.StatusCode ?? 400, result);
        }

        [HttpPost]
        [Authorize(Policy = "AdministratorOrModerator")]
        public async Task<IActionResult> CreateTank([FromBody] TankCreateDTO tankCreateDTO)
        {
            var result = await _tankService.CreateTankAsync(tankCreateDTO);
            return result.Success ? StatusCode(result.StatusCode ?? 200, result) : StatusCode(result.StatusCode ?? 400, result);
        }

        [HttpPatch("id/{tankId}")]
        [Authorize(Policy = "AdministratorOrModerator")]
        public async Task<IActionResult> UpdateTank(int tankId, [FromBody] TankUpdateDTO tankUpdateDTO)
        {
            Console.WriteLine($"User: {User.Identity.Name} | IsModerator?: {User.IsInRole("Moderator")}");
            var result = await _tankService.UpdateTankAsync(tankId, tankUpdateDTO);
            return result.Success ? StatusCode(result.StatusCode ?? 200, result) : StatusCode(result.StatusCode ?? 400, result);
        }

        [HttpDelete("id/{tankId}")]
        [Authorize(Policy = "AdministratorOrModerator")]
        public async Task<IActionResult> DeleteTank(int tankId)
        {
            var result = await _tankService.DeleteTankAsync(tankId);
            return result.Success ? StatusCode(result.StatusCode ?? 200, result) : StatusCode(result.StatusCode ?? 400, result);
        }
    }
}
