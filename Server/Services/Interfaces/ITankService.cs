using Server.Helpers;
using Server.Models.DTOs;

namespace Server.Services.Interfaces
{
    public interface ITankService
    {
        Task<ServiceResult<object>> GetAllTanksAsync(int pageNumber, int pageSize, List<string>? nationNames, List<string>? statusNames, List<string>? tankClassNames, List<int>? tiers, List<double>? ratings, string? searchTerm, string? sortBy, string? sortOrder);
        Task<ServiceResult<object>> GetTankByIdAsync(int tankId);
        Task<ServiceResult<object>> CreateTankAsync(TankCreateDTO tankCreateDTO);
        Task<ServiceResult<object>> UpdateTankAsync(int tankId, TankUpdateDTO tankUpdateDTO);
        Task<ServiceResult<object>> DeleteTankAsync(int tankId);
    }
}
