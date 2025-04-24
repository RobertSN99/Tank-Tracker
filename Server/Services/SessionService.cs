using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Helpers;
using Server.Models.DTOs;
using Server.Models.Entities;
using Server.Services.Interfaces;

namespace Server.Services
{
    public class SessionService : ISessionService
    {
        private readonly AppDbContext _context;

        public SessionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<List<UserSessionDTO>>> GetSessionsAsync()
        {
            var sessions = await _context.UserSessions.ToListAsync();
            var sessionDTOs = sessions.Select(MapToDTO).ToList();

            return ServiceResult<List<UserSessionDTO>>.SuccessResult(sessionDTOs);
        }

        public async Task<ServiceResult<UserSessionDTO>> GetSessionByIdAsync(string id)
        {
            // Validate the ID
            var idCheck = MyValidator.AgainstNullOrEmpty(id, nameof(id));
            if (!idCheck.Succeeded)
                return ServiceResult<UserSessionDTO>.FailureResult(idCheck.Errors.First().Description);

            // Check if the ID is a valid GUID
            if (!Guid.TryParse(id, out var sessionId))
                return ServiceResult<UserSessionDTO>.FailureResult($"'{id}' is not a valid session ID format.");

            var session = await _context.UserSessions.FindAsync(sessionId);
            if (session == null)
                return ServiceResult<UserSessionDTO>.FailureResult($"No session found with ID '{id}'.");

            return ServiceResult<UserSessionDTO>.SuccessResult(MapToDTO(session));
        }

        public async Task<ServiceResult<List<UserSessionDTO>>> GetSessionByUserIdAsync(string userId)
        {
            // Validate the user ID
            var idCheck = MyValidator.AgainstNullOrEmpty(userId, nameof(userId));
            if (!idCheck.Succeeded)
                return ServiceResult<List<UserSessionDTO>>.FailureResult(idCheck.Errors.First().Description);

            var sessions = await _context.UserSessions.ToListAsync();
            var sessionDTOs = sessions
                .Where(s => s.UserId == userId)
                .Select(MapToDTO)
                .ToList();

            return ServiceResult<List<UserSessionDTO>>.SuccessResult(sessionDTOs);
        }

        public async Task<ServiceResult<List<UserSessionDTO>>> GetSessionByRoleIdAsync(string roleId)
        {
            // Validate the role ID
            var idCheck = MyValidator.AgainstNullOrEmpty(roleId, nameof(roleId));
            if (!idCheck.Succeeded)
                return ServiceResult<List<UserSessionDTO>>.FailureResult(idCheck.Errors.First().Description);

            // Check if the role ID is a valid GUID
            var userIds = await _context.UserRoles
                .Where(r => r.RoleId == roleId)
                .Select(r => r.UserId)
                .ToListAsync();

            var sessions = await _context.UserSessions.ToListAsync();
            var sessionDTOs = sessions
                .Where(s => userIds.Contains(s.UserId))
                .Select(MapToDTO)
                .ToList();

            return ServiceResult<List<UserSessionDTO>>.SuccessResult(sessionDTOs);
        }

        public async Task<ServiceResult<object>> DeleteSessionAsync(string id)
        {
            // Validate the ID
            var idCheck = MyValidator.AgainstNullOrEmpty(id, nameof(id));
            if (!idCheck.Succeeded)
                return ServiceResult<object>.FailureResult(idCheck.Errors.First().Description);

            // Check if the ID is a valid GUID
            if (!Guid.TryParse(id, out var sessionId))
                return ServiceResult<object>.FailureResult($"'{id}' is not a valid session ID format.");

            // Check if the session exists
            var session = await _context.UserSessions.FindAsync(sessionId);
            if (session == null)
                return ServiceResult<object>.FailureResult($"No session found with ID '{id}'.");

            _context.UserSessions.Remove(session);
            await _context.SaveChangesAsync();

            return ServiceResult<object>.SuccessResult(null, $"Session '{id}' deleted successfully.");
        }

        // Helper method to map UserSession to UserSessionDTO
        private static UserSessionDTO MapToDTO(UserSession session)
        {
            return new UserSessionDTO
            {
                Id = session.Id,
                UserId = session.UserId,
                IpAddress = session.IPAddress,
                UserAgent = session.UserAgent,
                LoginTime = session.LoginTime,
                ExpirationTime = session.ExpirationTime,
                LogoutTime = session.LogoutTime
            };
        }
    }
}