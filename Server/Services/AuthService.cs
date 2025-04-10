using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Helper;
using Server.Helpers;
using Server.Models.DTOs;
using Server.Models.Entities;
using Server.Services.Interfaces;
using System.Security.Claims;

namespace Server.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, IHttpContextAccessor httpContextAccessor, AppDbContext context, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _configuration = configuration;
        }

        public async Task<ServiceResult<object>> RegisterAsync(RegisterDTO dto)
        {
            // Validate registration data
            var nullCheck = Guard.AgainstNull(dto, nameof(dto));
            if (!nullCheck.Succeeded)
                return ServiceResult<object>.FailureResult("Invalid registration data", nullCheck.Errors.Select(e => e.Description));

            var user = new User
            {
                UserName = dto.Username,
                Email = dto.Email,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return ServiceResult<object>.FailureResult("User registration failed", result.Errors.Select(e => e.Description));

            await _userManager.AddToRoleAsync(user, "User");

            return ServiceResult<object>.SuccessResult(user, "User registered successfully.");
        }

        public async Task<ServiceResult<object>> LoginAsync(LoginDTO dto)
        {
            // Validate login data
            var nullCheck = Guard.AgainstNull(dto, nameof(dto));
            if (!nullCheck.Succeeded)
                return ServiceResult<object>.FailureResult("Invalid login data", nullCheck.Errors.Select(e => e.Description));

            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return ServiceResult<object>.FailureResult("Invalid credentials");

            var passwordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!passwordValid)
                return ServiceResult<object>.FailureResult("Invalid credentials");

            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
                return ServiceResult<object>.FailureResult("No HTTP context available");

            var ipAddress = httpContext.Connection.RemoteIpAddress?.ToString();
            var userAgent = httpContext.Request.Headers["User-Agent"].ToString();

            var session = new UserSession
            {
                UserId = user.Id,
                IPAddress = ipAddress,
                UserAgent = userAgent,
                LoginTime = DateTime.UtcNow,
                ExpirationTime = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("SessionOptions:SessionDurationMinutes"))
            };

            _context.UserSessions.Add(session);
            await _context.SaveChangesAsync();

            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim("SessionId", session.Id.ToString())
            };

            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var identity = new ClaimsIdentity(claims, "Identity.Application");
            var principal = new ClaimsPrincipal(identity);

            await httpContext.SignInAsync("Identity.Application", principal, new AuthenticationProperties
            {
                IsPersistent = false,
                ExpiresUtc = session.ExpirationTime
            });

            var resultData = new
            {
                SessionId = session.Id,
                User = new
                {
                    user.Id,
                    user.UserName,
                    user.Email,
                    user.CreatedAt,
                    Roles = roles.ToList()
                }
            };

            return ServiceResult<object>.SuccessResult(resultData, "Login successful");
        }

        public async Task<ServiceResult<object>> LogoutAsync()
        {
            var userClaims = _httpContextAccessor.HttpContext?.User;
            if (userClaims == null)
                return ServiceResult<object>.FailureResult("No authenticated user found");

            var userId = userClaims.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return ServiceResult<object>.FailureResult("User ID not found in claims");

            var lastSession = await _context.UserSessions
                .Where(s => s.UserId == userId && s.LogoutTime == null)
                .OrderByDescending(s => s.LoginTime)
                .FirstOrDefaultAsync();

            if (lastSession != null)
            {
                lastSession.LogoutTime = DateTime.UtcNow;
                _context.UserSessions.Update(lastSession);
                await _context.SaveChangesAsync();
            }

            await _signInManager.SignOutAsync();

            return ServiceResult<object>.SuccessResult(null, "User logged out successfully");
        }
    }
}
