using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MeteorCRM.API.Core.Entities;

namespace MeteorCRM.API.Infrastructure.Data
{
    public class MeteorDbContext : IdentityDbContext<ApplicationUser>
    {
        public MeteorDbContext(DbContextOptions<MeteorDbContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Deal> Deals { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Soft Delete Filter
            builder.Entity<Customer>().HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<Deal>().HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<TaskItem>().HasQueryFilter(x => !x.IsDeleted);

            // Customer
            builder.Entity<Customer>(entity =>
            {
                entity.HasOne(c => c.User)
                    .WithMany(u => u.Customers)
                    .HasForeignKey(c => c.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Deal
            builder.Entity<Deal>(entity =>
            {
                entity.Property(d => d.Value)
                      .HasColumnType("decimal(18,2)");

                entity.HasOne(d => d.Customer)
                    .WithMany(c => c.Deals)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.User)
                    .WithMany(u => u.Deals)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // TaskItem
            builder.Entity<TaskItem>(entity =>
            {
                entity.HasOne(t => t.Customer)
                    .WithMany(c => c.Tasks)
                    .HasForeignKey(t => t.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.User)
                    .WithMany(u => u.Tasks)
                    .HasForeignKey(t => t.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // RefreshToken
            builder.Entity<RefreshToken>(entity =>
            {
                entity.HasOne(r => r.User)
                    .WithMany()
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}