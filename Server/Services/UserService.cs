using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Helpers;
using Server.Models.DTOs;
using Server.Models.Entities;
using Server.Services.Interfaces;
using Server.Helper;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;

namespace Server.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _roleManager = roleManager;
        }

        public async Task<ServiceResult<object>> GetAllUsersAsync(int page = 1, int pageSize = 10, string? search = null, string? role = null)
        {
            var query = _userManager.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(u =>
                    u.UserName!.Contains(search) ||
                    u.Email!.Contains(search));
            }

            if (!string.IsNullOrWhiteSpace(role))
            {
                var usersInRole = await _userManager.GetUsersInRoleAsync(role);
                query = query.Where(u => usersInRole.Select(ur => ur.Id).Contains(u.Id));
            }

            var totalUsers = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalUsers / (double)pageSize);

            var users = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var userDTOs = new List<UserDTO>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userDTOs.Add(new UserDTO
                {
                    Id = user.Id,
                    UserName = user!.UserName!,
                    Email = user!.Email!,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt,
                    Roles = roles.ToList()
                });
            }

            var resultData = new
            {
                Users = userDTOs,
                Pagination = new
                {
                    Page = page,
                    PageSize = pageSize,
                    TotalUsers = totalUsers,
                    TotalPages = totalPages
                }
            };

            return ServiceResult<object>.SuccessResult(resultData);
        }


        public async Task<ServiceResult<object>> AssignRoleToUserAsync(string userId, string roleId)
        {
            // Validate the userId parameter
            var userValidationResult = await IdentityValidator.ValidateUserByIdAsync(userId, _userManager);
            if (!userValidationResult.Success) return userValidationResult;

            // Validate the roleId parameter
            var roleValidationResult = await IdentityValidator.ValidateRoleByIdAsync(roleId, _roleManager);
            if (!roleValidationResult.Success) return roleValidationResult;

            // Check if the user already has the role
            var user = (User)userValidationResult.Data!;
            var role = (IdentityRole)roleValidationResult.Data!;
            var userRoles = await _userManager.GetRolesAsync(user);

            if (userRoles.Contains(role.Name!))
            {
                return ServiceResult<object>.FailureResult($"User '{user.UserName}' already has the role '{role.Name}'", [$"User '{user.UserName}' already has the role '{role.Name}'"]);
            }

            // Assign the role to the user
            var result = await _userManager.AddToRoleAsync(user, role.Name!);
            if (!result.Succeeded) 
                return ServiceResult<object>.FailureResult($"Failed to assign role '{role.Name}' to user '{user.UserName}'", result.Errors.Select(e => e.Description));
            
            return ServiceResult<object>.SuccessResult(role, $"Role '{role.Name}' assigned to user '{user!.UserName}' successfully.");
        }

        public async Task<ServiceResult<object>> ChangePasswordAsync(string userId, UserPasswordChangeDTO dto)
        {
            // Validate the userId parameter
            var userValidationResult = await IdentityValidator.ValidateUserByIdAsync(userId, _userManager);
            if (!userValidationResult.Success) return userValidationResult;

            // Validate the dto.OldPassword
            var oldPasswordNullCheck = Guard.AgainstNullOrEmpty(dto.OldPassword, nameof(dto.OldPassword));
            if (!oldPasswordNullCheck.Succeeded) return ServiceResult<object>.FailureResult(oldPasswordNullCheck.Errors.First().Description, oldPasswordNullCheck.Errors.Select(e => e.Description));

            // Validate the dto.NewPassword
            var newPasswordNullCheck = Guard.AgainstNullOrEmpty(dto.NewPassword, nameof(dto.NewPassword));
            if (!newPasswordNullCheck.Succeeded) return ServiceResult<object>.FailureResult(newPasswordNullCheck.Errors.First().Description, newPasswordNullCheck.Errors.Select(e => e.Description));

            // Change the password
            var user = (User)userValidationResult.Data!;
            var result = await _userManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);
            if (!result.Succeeded) 
                return ServiceResult<object>.FailureResult($"Failed to change password for user '{user.UserName}'", result.Errors.Select(e => e.Description));
            
            return ServiceResult<object>.SuccessResult(user, $"Password changed successfully for user '{user.UserName}'");
        }

        public async Task<ServiceResult<object>> DeleteUserAsync(string userId)
        {
            // Validate the userId parameter
            var userValidationResult = await IdentityValidator.ValidateUserByIdAsync(userId, _userManager);
            if (!userValidationResult.Success) return userValidationResult;

            // Delete the user
            var user = (User)userValidationResult.Data!;
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded) 
                return ServiceResult<object>.FailureResult($"Failed to delete user '{user.UserName}'", result.Errors.Select(e => e.Description));
            
            return ServiceResult<object>.SuccessResult(user, $"User '{user.UserName}' deleted successfully.");
        }

        public async Task<ServiceResult<UserDTO>> GetUserByEmailAsync(string email)
        {
            // Validate the the email parameter
            var userValidationResult = await IdentityValidator.ValidateUserByEmailAsync(email, _userManager);
            if (!userValidationResult.Success) 
                return ServiceResult<UserDTO>.FailureResult(userValidationResult.Message!, userValidationResult.Errors);

            var user = (User)userValidationResult.Data!;

            // Get the roles of the user
            var roles = await _userManager.GetRolesAsync(user!);

            return ServiceResult<UserDTO>.SuccessResult(new UserDTO
            {
                Id = user.Id,
                UserName = user.UserName!,
                Email = user.Email!,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                Roles = roles.ToList()
            });
        }

        public async Task<ServiceResult<UserDTO>> GetUserByIdAsync(string userId)
        {
            // Validate the userId parameter
            var uservalidationResult = await IdentityValidator.ValidateUserByIdAsync(userId, _userManager);
            if (!uservalidationResult.Success) 
                return ServiceResult<UserDTO>.FailureResult(uservalidationResult.Message!, uservalidationResult.Errors);

            // Get the roles of the user
            var user = (User)uservalidationResult.Data!;
            var roles = await _userManager.GetRolesAsync(user!);
            return ServiceResult<UserDTO>.SuccessResult(new UserDTO
            {
                Id = user!.Id,
                UserName = user!.UserName!,
                Email = user!.Email!,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                Roles = roles.ToList()
            });
        }

        public async Task<ServiceResult<object>> RemoveRoleFromUserAsync(string userId, string roleId)
        {
            // Validate the userId parameter
            var userValidationResult = await IdentityValidator.ValidateUserByIdAsync(userId, _userManager);
            if (!userValidationResult.Success) return userValidationResult;

            // Validate the roleId parameter
            var roleValidationResult = await IdentityValidator.ValidateRoleByIdAsync(roleId, _roleManager);
            if (!roleValidationResult.Success) return roleValidationResult;
            // Remove the role from the user
            var user = (User)userValidationResult.Data!;
            var role = (IdentityRole)roleValidationResult.Data!;

            var result = await _userManager.RemoveFromRoleAsync(user, role.Name!);
            if (!result.Succeeded) 
                return ServiceResult<object>.FailureResult($"Failed to remove role '{role.Name}' from user '{user.UserName}'", result.Errors.Select(e => e.Description));
            
            return ServiceResult<object>.SuccessResult(role, $"Role '{role.Name}' removed from user '{user.UserName}' successfully.");
        }

        public async Task<ServiceResult<UserDTO>> UpdateUserAsync(string userId, UserUpdateDTO dto)
        {
            // Validate the userId parameter
            var userValidationResult = await IdentityValidator.ValidateUserByIdAsync(userId, _userManager);
            if (!userValidationResult.Success) return ServiceResult<UserDTO>.FailureResult(userValidationResult.Message!, userValidationResult.Errors);

            var user = (User)userValidationResult.Data!;
            bool updated = false;

            if (!string.IsNullOrEmpty(dto.UserName) && dto.UserName != user.UserName)
            {
                user.UserName = dto.UserName;
                updated = true;
            }

            if (!string.IsNullOrEmpty(dto.Email) && dto.Email != user.Email)
            {
                user.Email = dto.Email;
                updated = true;
            }

            if (updated)
            {
                user.UpdatedAt = DateTime.UtcNow;
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                    return ServiceResult<UserDTO>.FailureResult("Failed to update user", result.Errors.Select(e => e.Description));
            }

            var roles = await _userManager.GetRolesAsync(user);
            return ServiceResult<UserDTO>.SuccessResult(new UserDTO
            {
                Id = user.Id,
                UserName = user!.UserName!,
                Email = user!.Email!,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                Roles = roles.ToList()
            });
        }

        public async Task<ServiceResult<object>> GetMyUserDetailsAsync()
        {
            var claimsUser = _httpContextAccessor.HttpContext?.User;
            if (claimsUser == null)
                return ServiceResult<object>.FailureResult("No user context found");

            var userId = claimsUser.FindFirstValue(ClaimTypes.NameIdentifier);

            // Validate the userId parameter
            var userValidationResult = await IdentityValidator.ValidateUserByIdAsync(userId!, _userManager);
            if (!userValidationResult.Success) return userValidationResult;

            var user = (User)userValidationResult.Data!;

            var roles = await _userManager.GetRolesAsync(user);

            var data = new
            {
                user.Id,
                user.UserName,
                user.Email,
                user.CreatedAt,
                Roles = roles.ToList()
            };

            return ServiceResult<object>.SuccessResult(data);
        }
    }
}
