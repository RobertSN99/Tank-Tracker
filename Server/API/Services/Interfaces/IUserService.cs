using API.Models.DTOs;
using API.Helpers;

namespace API.Services.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResult<object>> GetUserByIdAsync(string userId);
        Task<ServiceResult<object>> GetUserByEmailAsync(string email);
        Task<ServiceResult<object>> GetUserByNameAsync(string userName);
        Task<ServiceResult<object>> GetAllUsersAsync(int page, int pageSize, string? search, string? role);
        Task<ServiceResult<object>> UpdateUserAsync(string userId, UserUpdateDTO userDTO);
        Task<ServiceResult<object>> DeleteUserAsync(string userId);
        Task<ServiceResult<object>> AssignRoleToUserAsync(string userId, string roleId);
        Task<ServiceResult<object>> RemoveRoleFromUserAsync(string userId, string roleId);
        Task<ServiceResult<object>> ChangePasswordAsync(string userId, UserPasswordChangeDTO userPasswordChangeDTO);
        Task<ServiceResult<object>> GetMyUserDetailsAsync();
    }
}
