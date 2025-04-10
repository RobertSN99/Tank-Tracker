using Microsoft.AspNetCore.Identity;
using Server.Helpers;
using Server.Models.DTOs;

namespace Server.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResult<object>> RegisterAsync(RegisterDTO registerDTO);
        Task<ServiceResult<object>> LoginAsync(LoginDTO loginDTO);
        Task<ServiceResult<object>> LogoutAsync();
    }
}
