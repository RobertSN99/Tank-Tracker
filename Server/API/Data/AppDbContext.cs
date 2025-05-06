using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using API.Models.Entities;

namespace API.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        } 
        
        public DbSet<Tank> Tanks { get; set; }
        public DbSet<Nation> Nations { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<TankClass> TankClasses { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Tank>()
                .HasOne(t => t.Nation)
                .WithMany(n => n.Tanks)
                .HasForeignKey(t => t.NationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Tank>()
                .HasOne(t => t.TankClass)
                .WithMany(tc => tc.Tanks)
                .HasForeignKey(t => t.TankClassId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Tank>()
                .HasOne(t => t.Status)
                .WithMany(s => s.Tanks)
                .HasForeignKey(t => t.StatusId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserSession>()
                .HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
