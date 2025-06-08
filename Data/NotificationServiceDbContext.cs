using Microsoft.EntityFrameworkCore;
using NotificationService.API.Models;

namespace NotificationService.API.Data
{
    public class NotificationServiceDbContext : DbContext
    {
        public NotificationServiceDbContext(DbContextOptions<NotificationServiceDbContext> options)
            : base(options)
        {
        }

        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Message).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Type).IsRequired().HasMaxLength(50);
                
                // Add index for faster queries
                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => new { e.UserId, e.IsRead });
            });
        }
    }
} 