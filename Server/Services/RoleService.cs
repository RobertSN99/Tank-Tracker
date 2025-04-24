using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Server.Helpers;
using Server.Services.Interfaces;

public class RoleService : IRoleService
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public RoleService(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<ServiceResult<object>> CreateRoleAsync(string roleName)
    {
        // Validate role by name
        var validationResult = await MyValidator.ValidateRoleByNameAsync(roleName, _roleManager);
        if (validationResult != null) return validationResult;

        // Create the new role
        var role = new IdentityRole(roleName);
        var result = await _roleManager.CreateAsync(role);
        if (!result.Succeeded) 
            return ServiceResult<object>.FailureResult(result.Errors.First().Description, result.Errors.Select(e => e.Description));

        return ServiceResult<object>.SuccessResult(role, "Role created successfully.", 201);
    }

    public async Task<ServiceResult<object>> DeleteRoleAsync(string roleId)
    {
        // Validate role by ID
        var validationResult = await MyValidator.ValidateRoleByIdAsync(roleId, _roleManager);
        if (validationResult != null) return validationResult;

        var role = await _roleManager.FindByIdAsync(roleId);
        var result = await _roleManager.DeleteAsync(role!);
        if (!result.Succeeded) 
            return ServiceResult<object>.FailureResult(result.Errors.First().Description, result.Errors.Select(e => e.Description));

        return ServiceResult<object>.SuccessResult(null, "Role deleted successfully.", 204);
    }

    public async Task<ServiceResult<object>> GetRoleByIdAsync(string roleId)
    {
        // Validate role by ID
        var validationResult = await MyValidator.ValidateRoleByIdAsync(roleId, _roleManager);
        if (validationResult != null) return validationResult;

        var role = await _roleManager.FindByIdAsync(roleId);

        return ServiceResult<object>.SuccessResult(role, "Role retrieved successfully.");
    }

    public async Task<ServiceResult<object>> GetRoleByNameAsync(string roleName)
    {
        // Validate role by name
        var validationResult = await MyValidator.ValidateRoleByNameAsync(roleName, _roleManager);
        if (validationResult != null) return validationResult;

        var role = await _roleManager.FindByNameAsync(roleName);

        return ServiceResult<object>.SuccessResult(role, "Role retrieved successfully.");
    }

    public async Task<ServiceResult<object>> GetAllRolesAsync()
    {
        // Retrieve all roles from the database
        var roles = await _roleManager.Roles.ToListAsync();

        if (roles == null || roles.Count == 0)
        {
            return ServiceResult<object>.FailureResult("No roles found.");
        }

        return ServiceResult<object>.SuccessResult(roles);
    }

    public async Task<ServiceResult<object>> UpdateRoleAsync(string roleId, string roleName)
    {
        // Validate role by ID (reuse helper method)
        var idValidationResult = await MyValidator.ValidateRoleByIdAsync(roleId, _roleManager);
        if (idValidationResult != null) return idValidationResult;

        // Validate role by name (reuse helper method)
        var nameValidationResult = await MyValidator.ValidateRoleByNameAsync(roleName, _roleManager);
        if (nameValidationResult != null) return nameValidationResult;

        var role = await _roleManager.FindByIdAsync(roleId);
        role!.Name = roleName;

        var result = await _roleManager.UpdateAsync(role);
        if (!result.Succeeded) 
            return ServiceResult<object>.FailureResult($"Failed to update role '{roleId}'", result.Errors.Select(e => e.Description));

        return ServiceResult<object>.SuccessResult(role, $"Role '{roleId}' updated successfully.");
    }
}
