﻿using Server.Models.DTOs;
using Server.Helpers;

namespace Server.Services.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResult<UserDTO>> GetUserByIdAsync(string userId);
        Task<ServiceResult<UserDTO>> GetUserByEmailAsync(string email);
        Task<ServiceResult<object>> GetAllUsersAsync(int page, int pageSize, string? search, string? role);
        Task<ServiceResult<UserDTO>> UpdateUserAsync(string userId, UserUpdateDTO userDTO);
        Task<ServiceResult<object>> DeleteUserAsync(string userId);
        Task<ServiceResult<object>> AssignRoleToUserAsync(string userId, string roleId);
        Task<ServiceResult<object>> RemoveRoleFromUserAsync(string userId, string roleId);
        Task<ServiceResult<object>> ChangePasswordAsync(string userId, UserPasswordChangeDTO userPasswordChangeDTO);
        Task<ServiceResult<object>> GetMyUserDetailsAsync();
    }
}
