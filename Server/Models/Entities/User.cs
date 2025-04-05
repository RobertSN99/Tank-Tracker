using Microsoft.AspNetCore.Identity;

namespace Server.Models.Entities
{
    public class User : IdentityUser
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = null;
    }
}
