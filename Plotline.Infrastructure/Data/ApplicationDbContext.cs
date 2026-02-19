using Microsoft.EntityFrameworkCore;
using Plotline.Core.Models;

namespace Plotline.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");

                entity.Property(e => e.Login)
                    .HasMaxLength(100)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsRequired()
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.PasswordHash)
                    .HasMaxLength(255)
                    .IsRequired();

                entity.Property(e => e.DisplayName)
                    .HasMaxLength(255)
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("NOW()");

                entity.Property(e => e.UpdatedAt)
                    .HasDefaultValueSql("NOW()");

                entity.HasIndex(e => e.Login).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.CreatedAt);

                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(e => e.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(e => e.UpdatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasIndex(e => e.TokenHash).IsUnique();
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.TokenHash)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.IpAddress)
                    .HasMaxLength(45);

                entity.Property(e => e.UserAgent)
                    .HasMaxLength(500);

                entity.HasOne(rt => rt.User)
                    .WithMany(u => u.RefreshTokens)
                    .HasForeignKey(rt => rt.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}