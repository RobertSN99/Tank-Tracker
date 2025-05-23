﻿using Microsoft.AspNetCore.Identity;
using API.Helpers;
using API.Models.Entities;

namespace API.Services.Interfaces
{
    public interface IRoleService
    {
        Task<ServiceResult<object>> GetAllRolesAsync();
        Task<ServiceResult<object>> GetRoleByIdAsync(string roleId);
        Task<ServiceResult<object>> GetRoleByNameAsync(string roleName);
        Task<ServiceResult<object>> CreateRoleAsync(string roleName);
        Task<ServiceResult<object>> UpdateRoleAsync(string roleId, string roleName);
        Task<ServiceResult<object>> DeleteRoleAsync(string roleId);
    }
}
