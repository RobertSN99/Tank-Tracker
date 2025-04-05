using Microsoft.AspNetCore.Identity;
using Server.Services.Interfaces;

namespace Server.Services
{
    public class AdminService : IAdminService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<IdentityRole> CreateRoleAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Role name cannot be null or empty.", nameof(name));
            }

            if (await _roleManager.RoleExistsAsync(name))
            {
                throw new InvalidOperationException($"Role '{name}' already exists.");
            }

            var result = await _roleManager.CreateAsync(new IdentityRole(name));
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Failed to create role '{name}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
            return await _roleManager.FindByNameAsync(name);
        }

        public async Task<bool> DeleteRoleAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                throw new InvalidOperationException($"Role with ID '{id}' does not exist.");
            }
            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Failed to delete role '{role.Name}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
            return true;
        }

        public Task<IdentityRole> GetRoleByIdAsync(string id)
        {
            var role = _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                throw new InvalidOperationException($"Role with ID '{id}' does not exist.");
            }
            return Task.FromResult(role.Result);
        }

        public Task<IdentityRole> GetRoleByNameAsync(string name)
        {
            var role = _roleManager.FindByNameAsync(name);
            if (role == null)
            {
                throw new InvalidOperationException($"Role with name '{name}' does not exist.");
            }
            return Task.FromResult(role.Result);
        }

        public Task<IList<IdentityRole>> GetRolesAsync()
        {
            var roles = _roleManager.Roles.ToList();
            if (roles == null || !roles.Any())
            {
                return Task.FromResult<IList<IdentityRole>>(new List<IdentityRole>());
            }
            return Task.FromResult<IList<IdentityRole>>(roles);
        }

        public Task<IdentityRole> UpdateRoleAsync(string id, string name)
        {
            var role = _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                throw new InvalidOperationException($"Role with ID '{id}' does not exist.");
            }
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Role name cannot be null or empty.", nameof(name));
            }
            if (role.Result.Name == name)
            {
                throw new InvalidOperationException($"Role with ID '{id}' already has the name '{name}'.");
            }
            role.Result.Name = name;
            var result = _roleManager.UpdateAsync(role.Result);
            if (!result.Result.Succeeded)
            {
                throw new InvalidOperationException($"Failed to update role '{role.Result.Name}': {string.Join(", ", result.Result.Errors.Select(e => e.Description))}");
            }
            return Task.FromResult(role.Result);
        }
    }
}
