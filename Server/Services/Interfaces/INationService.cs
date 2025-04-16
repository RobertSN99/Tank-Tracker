using Server.Helpers;
using Server.Models.DTOs;

namespace Server.Services.Interfaces
{
    public interface INationService
    {
        Task<ServiceResult<object>> GetAllNationsAsync();
        Task<ServiceResult<object>> GetNationByIdAsync(int nationId);
        Task<ServiceResult<object>> CreateNationAsync(NationCreateDTO nationDto);
        Task<ServiceResult<object>> UpdateNationAsync(int nationId, NationUpdateDTO nationDto);
        Task<ServiceResult<object>> DeleteNationAsync(int nationId);
    }
}
