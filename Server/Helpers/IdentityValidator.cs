using Microsoft.AspNetCore.Identity;
using Server.Helper;
using Server.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace Server.Helpers
{
    public class IdentityValidator
    {

        // Validates if a user exists by userId.
        public static async Task<ServiceResult<object>> ValidateUserByIdAsync(string userId, UserManager<User> userManager)
        {
            var userIdNullCheck = Guard.AgainstNullOrEmpty(userId, nameof(userId));
            if (!userIdNullCheck.Succeeded)
                return ServiceResult<object>.FailureResult(userIdNullCheck.Errors.First().Description, userIdNullCheck.Errors.Select(e => e.Description));

            var user = await userManager.FindByIdAsync(userId);
            var existsCheck = Guard.AgainstCondition(user == null, $"User with ID '{userId}' does not exist.");
            if (!existsCheck.Succeeded)
                return ServiceResult<object>.FailureResult(existsCheck.Errors.First().Description, existsCheck.Errors.Select(e => e.Description));

            return ServiceResult<object>.SuccessResult(user);
        }

        // Validates if a user exists by email.
        public static async Task<ServiceResult<object>> ValidateUserByEmailAsync(string email, UserManager<User> userManager)
        {
            var emailNullCheck = Guard.AgainstNullOrEmpty(email, nameof(email));
            if (!emailNullCheck.Succeeded)
                return ServiceResult<object>.FailureResult(emailNullCheck.Errors.First().Description, emailNullCheck.Errors.Select(e => e.Description));

            // Email format validation
            var emailFormatCheck = new EmailAddressAttribute().IsValid(email);
            if (!emailFormatCheck)
                return ServiceResult<object>.FailureResult($"Email '{email}' is not in a valid format.");

            var user = await userManager.FindByEmailAsync(email);
            var existsCheck = Guard.AgainstCondition(user == null, $"User with email '{email}' does not exist.");
            if (!existsCheck.Succeeded)
                return ServiceResult<object>.FailureResult(existsCheck.Errors.First().Description, existsCheck.Errors.Select(e => e.Description));

            return ServiceResult<object>.SuccessResult(user);
        }

        // Validates if a role exists by roleId.
        public static async Task<ServiceResult<object>> ValidateRoleByIdAsync(string roleId, RoleManager<IdentityRole> roleManager)
        {
            var roleIdNullCheck = Guard.AgainstNullOrEmpty(roleId, nameof(roleId));
            if (!roleIdNullCheck.Succeeded)
                return ServiceResult<object>.FailureResult(roleIdNullCheck.Errors.First().Description, roleIdNullCheck.Errors.Select(e => e.Description));
            
            var role = await roleManager.FindByIdAsync(roleId);
            var existsCheck = Guard.AgainstCondition(role == null, $"Role with ID '{roleId}' does not exist.");
            if (!existsCheck.Succeeded)
                return ServiceResult<object>.FailureResult(existsCheck.Errors.First().Description, existsCheck.Errors.Select(e => e.Description));
            
            return ServiceResult<object>.SuccessResult(role);
        }

        // Validates if a role exists by role name.
        public static async Task<ServiceResult<object>> ValidateRoleByNameAsync(string roleName, RoleManager<IdentityRole> roleManager)
        {
            var roleNameNullCheck = Guard.AgainstNullOrEmpty(roleName, nameof(roleName));
            if (!roleNameNullCheck.Succeeded)
                return ServiceResult<object>.FailureResult(roleNameNullCheck.Errors.First().Description, roleNameNullCheck.Errors.Select(e => e.Description));
            
            var role = await roleManager.FindByNameAsync(roleName);
            var existsCheck = Guard.AgainstCondition(role == null, $"Role '{roleName}' does not exist.");
            if (!existsCheck.Succeeded)
                return ServiceResult<object>.FailureResult(existsCheck.Errors.First().Description, existsCheck.Errors.Select(e => e.Description));
            
            return ServiceResult<object>.SuccessResult(role);
        }
    }
}
