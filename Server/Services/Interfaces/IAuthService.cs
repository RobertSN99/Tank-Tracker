using Microsoft.AspNetCore.Identity;
using Server.Models.DTOs;

namespace Server.Services.Interfaces
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterAsync(RegisterDTO registerDTO);
        Task<SignInResult> LoginAsync(LoginDTO loginDTO);
        Task LogoutAsync();
    }
}
