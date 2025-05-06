using Microsoft.AspNetCore.Identity;
using API.Helpers;
using API.Models.DTOs;

namespace API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResult<object>> RegisterAsync(RegisterDTO registerDTO);
        Task<ServiceResult<object>> LoginAsync(LoginDTO loginDTO);
        Task<ServiceResult<object>> LogoutAsync();
    }
}
