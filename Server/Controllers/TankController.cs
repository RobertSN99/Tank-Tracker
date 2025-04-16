using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Models.DTOs;
using Server.Services.Interfaces;

namespace Server.Controllers
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
        public async Task<IActionResult> GetAllTanks([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            const int maxPageSize = 50;
            if (pageSize > maxPageSize)
                return BadRequest($"Page size cannot exceed {maxPageSize}.");

            var result = await _tankService.GetAllTanksAsync(pageNumber, pageSize);
            return result.Success ? Ok(result.Data) : NotFound(result.Errors);
        }

        [HttpGet("id/{tankId}")]
        public async Task<IActionResult> GetTankById(int tankId)
        {
            var result = await _tankService.GetTankByIdAsync(tankId);
            return result.Success ? Ok(result.Data) : NotFound(result.Errors);
        }

        [HttpPost]
        [Authorize(Policy = "AdministratorOrModerator")]
        public async Task<IActionResult> CreateTank([FromBody] TankCreateDTO tankCreateDTO)
        {
            var result = await _tankService.CreateTankAsync(tankCreateDTO);
            return result.Success ? CreatedAtAction(nameof(GetTankById), new { tankId = result.Data }, result.Data) : BadRequest(result.Errors);
        }

        [HttpPatch("id/{tankId}")]
        [Authorize(Policy = "AdministratorOrModerator")]
        public async Task<IActionResult> UpdateTank(int tankId, [FromBody] TankUpdateDTO tankUpdateDTO)
        {
            var result = await _tankService.UpdateTankAsync(tankId, tankUpdateDTO);
            return result.Success ? Ok(result.Data) : NotFound(result.Errors);
        }

        [HttpDelete("id/{tankId}")]
        [Authorize(Policy = "AdministratorOrModerator")]
        public async Task<IActionResult> DeleteTank(int tankId)
        {
            var result = await _tankService.DeleteTankAsync(tankId);
            return result.Success ? NoContent() : NotFound(result.Errors);
        }
    }
}
