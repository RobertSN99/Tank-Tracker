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
            return result.Success ? StatusCode(result.StatusCode ?? 200, result) : StatusCode(result.StatusCode ?? 400, result);
        }

        [HttpPost]
        [Authorize(Policy = "AdministratorOrModerator")]
        public async Task<IActionResult> CreateStatus([FromBody] StatusCreateDTO statusDto)
        {
            var result = await _statusService.CreateStatusAsync(statusDto);
            return result.Success ? StatusCode(result.StatusCode ?? 200, result) : StatusCode(result.StatusCode ?? 400, result);
        }

        [HttpGet("id/{statusId}")]
        public async Task<IActionResult> GetStatusById(int statusId)
        {
            var result = await _statusService.GetStatusByIdAsync(statusId);
            return result.Success ? StatusCode(result.StatusCode ?? 200, result) : StatusCode(result.StatusCode ?? 400, result);
        }

        [HttpPatch("id/{statusId}")]
        [Authorize(Policy = "AdministratorOrModerator")]
        public async Task<IActionResult> UpdateStatus(int statusId, [FromBody] StatusUpdateDTO statusDto)
        {
            var result = await _statusService.UpdateStatusAsync(statusId, statusDto);
            return result.Success ? StatusCode(result.StatusCode ?? 200, result) : StatusCode(result.StatusCode ?? 400, result);
        }

        [HttpDelete("id/{statusId}")]
        [Authorize(Policy = "AdministratorOrModerator")]
        public async Task<IActionResult> DeleteStatus(int statusId)
        {
            var result = await _statusService.DeleteStatusAsync(statusId);
            return result.Success ? StatusCode(result.StatusCode ?? 200, result) : StatusCode(result.StatusCode ?? 400, result);
        }
    }
}
