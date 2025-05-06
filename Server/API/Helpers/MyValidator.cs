using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace API.Helpers
{
    public class MyValidator
    {

        // Validates if a string is null or empty.
        public static IdentityResult AgainstNullOrEmpty(string value, string fieldName)
        {
            return string.IsNullOrEmpty(value)
                ? IdentityResult.Failed(new IdentityError { Description = $"{fieldName} cannot be null or empty." })
                : IdentityResult.Success;
        }

        // Validates if a condition is true.
        public static IdentityResult AgainstCondition(bool condition, string errorMessage)
        {
            return condition
                ? IdentityResult.Failed(new IdentityError { Description = errorMessage })
                : IdentityResult.Success;
        }

        // Validates if an object is null.
        public static IdentityResult AgainstNull(object obj, string fieldName)
        {
            return obj == null
                ? IdentityResult.Failed(new IdentityError { Description = $"{fieldName} cannot be null." })
                : IdentityResult.Success;
        }

        // Validates if a user exists by userId.
        public static async Task<ServiceResult<object>> ValidateUserByIdAsync(string userId, UserManager<User> userManager)
        {
            var userIdNullCheck = AgainstNullOrEmpty(userId, nameof(userId));
            if (!userIdNullCheck.Succeeded)
                return ServiceResult<object>.FailureResult(userIdNullCheck.Errors.First().Description, userIdNullCheck.Errors.Select(e => e.Description));

            var user = await userManager.FindByIdAsync(userId);
            var existsCheck = AgainstCondition(user == null, $"User with ID '{userId}' does not exist.");
            if (!existsCheck.Succeeded)
                return ServiceResult<object>.FailureResult(existsCheck.Errors.First().Description, existsCheck.Errors.Select(e => e.Description), 404);

            return ServiceResult<object>.SuccessResult(user);
        }

        // Validates if a user exists by email.
        public static async Task<ServiceResult<object>> ValidateUserByEmailAsync(string email, UserManager<User> userManager)
        {
            var emailNullCheck = AgainstNullOrEmpty(email, nameof(email));
            if (!emailNullCheck.Succeeded)
                return ServiceResult<object>.FailureResult(emailNullCheck.Errors.First().Description, emailNullCheck.Errors.Select(e => e.Description));

            // Email format validation
            var emailFormatCheck = new EmailAddressAttribute().IsValid(email);
            if (!emailFormatCheck)
                return ServiceResult<object>.FailureResult($"Email '{email}' is not in a valid format.");

            var user = await userManager.FindByEmailAsync(email);
            var existsCheck = AgainstCondition(user == null, $"User with email '{email}' does not exist.");
            if (!existsCheck.Succeeded)
                return ServiceResult<object>.FailureResult(existsCheck.Errors.First().Description, existsCheck.Errors.Select(e => e.Description), 404);

            return ServiceResult<object>.SuccessResult(user);
        }

        // Validates if a user exists by username.
        public static async Task<ServiceResult<object>> ValidateUserByUsernameAsync(string username, UserManager<User> userManager)
        {
            var usernameNullCheck = AgainstNullOrEmpty(username, nameof(username));
            if (!usernameNullCheck.Succeeded)
                return ServiceResult<object>.FailureResult(usernameNullCheck.Errors.First().Description, usernameNullCheck.Errors.Select(e => e.Description));
            
            var user = await userManager.FindByNameAsync(username);
            var existsCheck = AgainstCondition(user == null, $"User with username '{username}' does not exist.");
            if (!existsCheck.Succeeded)
                return ServiceResult<object>.FailureResult(existsCheck.Errors.First().Description, existsCheck.Errors.Select(e => e.Description), 404);
            
            return ServiceResult<object>.SuccessResult(user);
        }

        // Validates if a role exists by roleId.
        public static async Task<ServiceResult<object>> ValidateRoleByIdAsync(string roleId, RoleManager<IdentityRole> roleManager)
        {
            var roleIdNullCheck = AgainstNullOrEmpty(roleId, nameof(roleId));
            if (!roleIdNullCheck.Succeeded)
                return ServiceResult<object>.FailureResult(roleIdNullCheck.Errors.First().Description, roleIdNullCheck.Errors.Select(e => e.Description));
            
            var role = await roleManager.FindByIdAsync(roleId);
            var existsCheck = AgainstCondition(role == null, $"Role with ID '{roleId}' does not exist.");
            if (!existsCheck.Succeeded)
                return ServiceResult<object>.FailureResult(existsCheck.Errors.First().Description, existsCheck.Errors.Select(e => e.Description), 404);
            
            return ServiceResult<object>.SuccessResult(role);
        }

        // Validates if a role exists by role name.
        public static async Task<ServiceResult<object>> ValidateRoleByNameAsync(string roleName, RoleManager<IdentityRole> roleManager)
        {
            var roleNameNullCheck = AgainstNullOrEmpty(roleName, nameof(roleName));
            if (!roleNameNullCheck.Succeeded)
                return ServiceResult<object>.FailureResult(roleNameNullCheck.Errors.First().Description, roleNameNullCheck.Errors.Select(e => e.Description));
            
            var role = await roleManager.FindByNameAsync(roleName);
            var existsCheck = AgainstCondition(role == null, $"Role '{roleName}' does not exist.");
            if (!existsCheck.Succeeded)
                return ServiceResult<object>.FailureResult(existsCheck.Errors.First().Description, existsCheck.Errors.Select(e => e.Description), 404);
            
            return ServiceResult<object>.SuccessResult(role);
        }

        // Validates if a status exists by ID.
        public static async Task<ServiceResult<object>> ValidateStatusByIdAsync(int statusId, AppDbContext context)
        {
            var statusIdCheck = AgainstCondition(statusId <= 0, $"Status ID must be greater than zero");
            if (!statusIdCheck.Succeeded)
                return ServiceResult<object>.FailureResult(statusIdCheck.Errors.First().Description, statusIdCheck.Errors.Select(e => e.Description));
            
            var status = await context.Statuses.FindAsync(statusId);
            var existsCheck = AgainstCondition(status == null, $"Status with ID '{statusId}' does not exist.");
            if (!existsCheck.Succeeded)
                return ServiceResult<object>.FailureResult(existsCheck.Errors.First().Description, existsCheck.Errors.Select(e => e.Description), 404);
            
            return ServiceResult<object>.SuccessResult(status);
        }

        // Validates if a status exists by name.
        public static async Task<ServiceResult<object>> ValidateStatusByNameAsync(string statusName, AppDbContext context)
        {
            var statusNameCheck = AgainstNullOrEmpty(statusName, nameof(statusName));
            if (!statusNameCheck.Succeeded)
                return ServiceResult<object>.FailureResult(statusNameCheck.Errors.First().Description, statusNameCheck.Errors.Select(e => e.Description));
            
            var status = await context.Statuses.FirstOrDefaultAsync(s => s.Name == statusName);
            var existsCheck = AgainstCondition(status == null, $"Status with name '{statusName}' does not exist.");
            if (!existsCheck.Succeeded)
                return ServiceResult<object>.FailureResult(existsCheck.Errors.First().Description, existsCheck.Errors.Select(e => e.Description), 404);
           
            return ServiceResult<object>.SuccessResult(status);
        }

        // Validates if a nation exists by ID.
        public static async Task<ServiceResult<object>> ValidateNationByIdAsync(int nationId, AppDbContext context)
        {
            var nationIdCheck = AgainstCondition(nationId <= 0, $"Nation ID must be greater than zero");
            if (!nationIdCheck.Succeeded)
                return ServiceResult<object>.FailureResult(nationIdCheck.Errors.First().Description, nationIdCheck.Errors.Select(e => e.Description));

            var nation = await context.Nations.FindAsync(nationId);
            var existsCheck = AgainstCondition(nation == null, $"Nation with ID '{nationId}' does not exist.");
            if (!existsCheck.Succeeded)
                return ServiceResult<object>.FailureResult(existsCheck.Errors.First().Description, existsCheck.Errors.Select(e => e.Description), 404);

            return ServiceResult<object>.SuccessResult(nation);
        }

        // Validates if a nation exists by name.
        public static async Task<ServiceResult<object>> ValidateNationByNameAsync(string nationName, AppDbContext context)
        {
            var nationNameCheck = AgainstNullOrEmpty(nationName, nameof(nationName));
            if (!nationNameCheck.Succeeded)
                return ServiceResult<object>.FailureResult(nationNameCheck.Errors.First().Description, nationNameCheck.Errors.Select(e => e.Description));
            
            var nation = await context.Nations.FirstOrDefaultAsync(n => n.Name == nationName);
            var existsCheck = AgainstCondition(nation == null, $"Nation with name '{nationName}' does not exist.");
            if (!existsCheck.Succeeded)
                return ServiceResult<object>.FailureResult(existsCheck.Errors.First().Description, existsCheck.Errors.Select(e => e.Description), 404);
            
            return ServiceResult<object>.SuccessResult(nation);
        }

        // Validates if a tank class exists by ID.
        public static async Task<ServiceResult<object>> ValidateTankClassByIdAsync(int tankClassId, AppDbContext context)
        {
            var tankClassIdCheck = AgainstCondition(tankClassId <= 0, $"Tank Class ID must be greater than zero");
            if (!tankClassIdCheck.Succeeded)
                return ServiceResult<object>.FailureResult(tankClassIdCheck.Errors.First().Description, tankClassIdCheck.Errors.Select(e => e.Description));

            var tankClass = await context.TankClasses.FindAsync(tankClassId);
            var existsCheck = AgainstCondition(tankClass == null, $"Tank Class with ID '{tankClassId}' does not exist.");
            if (!existsCheck.Succeeded)
                return ServiceResult<object>.FailureResult(existsCheck.Errors.First().Description, existsCheck.Errors.Select(e => e.Description), 404);

            return ServiceResult<object>.SuccessResult(tankClass);
        }

        // Validates if a tank class exists by name.
        public static async Task<ServiceResult<object>> ValidateTankClassByNameAsync(string tankClassName, AppDbContext context)
        {
            var tankClassNameCheck = AgainstNullOrEmpty(tankClassName, nameof(tankClassName));
            if (!tankClassNameCheck.Succeeded)
                return ServiceResult<object>.FailureResult(tankClassNameCheck.Errors.First().Description, tankClassNameCheck.Errors.Select(e => e.Description));

            var tankClass = await context.TankClasses.FirstOrDefaultAsync(tc => tc.Name == tankClassName);
            var existsCheck = AgainstCondition(tankClass == null, $"Tank Class with name '{tankClassName}' does not exist.");
            if (!existsCheck.Succeeded)
                return ServiceResult<object>.FailureResult(existsCheck.Errors.First().Description, existsCheck.Errors.Select(e => e.Description), 404);

            return ServiceResult<object>.SuccessResult(tankClass);
        }

        // Validates if a tank exists by ID.
        public static async Task<ServiceResult<object>> ValidateTankByIdAsync(int tankId, AppDbContext context)
        {
            var tankIdCheck = AgainstCondition(tankId <= 0, $"Tank ID must be greater than zero");
            if (!tankIdCheck.Succeeded)
                return ServiceResult<object>.FailureResult(tankIdCheck.Errors.First().Description, tankIdCheck.Errors.Select(e => e.Description));

            var tank = await context.Tanks
                .Include(t => t.Nation)
                .Include(t => t.TankClass)
                .Include(t => t.Status)
                .FirstOrDefaultAsync(t => t.Id == tankId);
            var existsCheck = AgainstCondition(tank == null, $"Tank with ID '{tankId}' does not exist.");
            if (!existsCheck.Succeeded)
                return ServiceResult<object>.FailureResult(existsCheck.Errors.First().Description, existsCheck.Errors.Select(e => e.Description), 404);

            return ServiceResult<object>.SuccessResult(tank);
        }

        // Validates if a tank exists by name.
        public static async Task<ServiceResult<object>> ValidateTankByNameAsync(string tankName, AppDbContext context)
        {
            var tankNameCheck = AgainstNullOrEmpty(tankName, nameof(tankName));
            if (!tankNameCheck.Succeeded)
                return ServiceResult<object>.FailureResult(tankNameCheck.Errors.First().Description, tankNameCheck.Errors.Select(e => e.Description));
            
            var tank = await context.Tanks
                .Include(t => t.Nation)
                .Include(t => t.TankClass)
                .Include(t => t.Status)
                .FirstOrDefaultAsync(t => t.Name == tankName);
            var existsCheck = AgainstCondition(tank == null, $"Tank with name '{tankName}' does not exist.");
            if (!existsCheck.Succeeded)
                return ServiceResult<object>.FailureResult(existsCheck.Errors.First().Description, existsCheck.Errors.Select(e => e.Description), 404);
            
            return ServiceResult<object>.SuccessResult(tank);
        }
    }
}
