using Server.Helpers;
using Server.Models.DTOs;

namespace Server.Services.Interfaces
{
    public interface ISessionService
    {
        Task<ServiceResult<List<UserSessionDTO>>> GetSessionsAsync();
        Task<ServiceResult<UserSessionDTO>> GetSessionByIdAsync(string id);
        Task<ServiceResult<List<UserSessionDTO>>> GetSessionByUserIdAsync(string userId);
        Task<ServiceResult<List<UserSessionDTO>>> GetSessionByRoleIdAsync(string roleId);
        Task<ServiceResult<object>> DeleteSessionAsync(string id);
    }
}
