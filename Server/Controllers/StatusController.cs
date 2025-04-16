using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Models.DTOs;
using Server.Services.Interfaces;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly IStatusService _statusService;

        public StatusController(IStatusService statusService)
        {
            _statusService = statusService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllStatuses()
        {
            var result = await _statusService.GetAllStatusesAsync();
            return result.Success ? Ok(result.Data) : NotFound(new { result.Message });
        }

        [HttpPost]
        [Authorize(Policy = "AdministratorOrModerator")]
        public async Task<IActionResult> CreateStatus([FromBody] StatusCreateDTO statusDto)
        {
            var result = await _statusService.CreateStatusAsync(statusDto);
            return result.Success ? Ok(result.Data) : BadRequest(new { result.Message });
        }

        [HttpGet("id/{statusId}")]
        public async Task<IActionResult> GetStatusById(int statusId)
        {
            var result = await _statusService.GetStatusByIdAsync(statusId);
            return result.Success ? Ok(result.Data) : NotFound(new { result.Message });
        }

        [HttpPatch("id/{statusId}")]
        [Authorize(Policy = "AdministratorOrModerator")]
        public async Task<IActionResult> UpdateStatus(int statusId, [FromBody] StatusUpdateDTO statusDto)
        {
            var result = await _statusService.UpdateStatusAsync(statusId, statusDto);
            return result.Success ? Ok(result.Data) : BadRequest(new { result.Message });
        }

        [HttpDelete("id/{statusId}")]
        [Authorize(Policy = "AdministratorOrModerator")]
        public async Task<IActionResult> DeleteStatus(int statusId)
        {
            var result = await _statusService.DeleteStatusAsync(statusId);
            return result.Success ? Ok(result.Data) : NotFound(new { result.Message });
        }
    }
}
