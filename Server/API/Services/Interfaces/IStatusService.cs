using API.Helpers;
using API.Models.DTOs;

namespace API.Services.Interfaces
{
    public interface IStatusService
    {
        Task<ServiceResult<object>> GetAllStatusesAsync();
        Task<ServiceResult<object>> GetStatusByIdAsync(int statusId);
        Task<ServiceResult<object>> CreateStatusAsync(StatusCreateDTO statusDto);
        Task<ServiceResult<object>> UpdateStatusAsync(int statusId, StatusUpdateDTO statusDto);
        Task<ServiceResult<object>> DeleteStatusAsync(int statusId);
    }
}
