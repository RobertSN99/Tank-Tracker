using Server.Helpers;
using Server.Models.DTOs;

namespace Server.Services.Interfaces
{
    public interface ITankClassService
    {
        Task<ServiceResult<object>> GetAllClassesAsync();
        Task<ServiceResult<object>> GetClassByIdAsync(int classId);
        Task<ServiceResult<object>> CreateClassAsync(TankClassCreateDTO tankClassDto);
        Task<ServiceResult<object>> UpdateClassAsync(int classId, TankClassUpdateDTO tankClassDto);
        Task<ServiceResult<object>> DeleteClassAsync(int classId);
    }
}
