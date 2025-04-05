using Microsoft.AspNetCore.Identity;

namespace Server.Services.Interfaces
{
    public interface IAdminService
    {
        Task<IList<IdentityRole>> GetRolesAsync();
        Task<IdentityRole> GetRoleByIdAsync(string id);
        Task<IdentityRole> GetRoleByNameAsync(string name);
        Task<IdentityRole> CreateRoleAsync(string name);
        Task<IdentityRole> UpdateRoleAsync(string id, string name);
        Task<bool> DeleteRoleAsync(string id);
    }
}
