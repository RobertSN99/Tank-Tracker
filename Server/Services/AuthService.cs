using Microsoft.AspNetCore.Identity;
using Server.Models.DTOs;
using Server.Models.Entities;
using Server.Services.Interfaces;

namespace Server.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterDTO registerDTO)
        {
            var user = new User
            {
                UserName = registerDTO.Username,
                Email = registerDTO.Email,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, registerDTO.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
            }
            return result;
        }
        public async Task<SignInResult> LoginAsync(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (user == null)
            {
                return SignInResult.Failed;
            }

            var result = await _signInManager.PasswordSignInAsync(user, loginDTO.Password, false, false);
            return result;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
