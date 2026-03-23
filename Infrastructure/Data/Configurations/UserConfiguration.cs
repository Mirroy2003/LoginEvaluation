using LoginEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoginEvaluation.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(256);

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.PasswordHash)
            .IsRequired();

        builder.Property(u => u.IsActive)
            .IsRequired();

        builder.Property(u => u.CreatedAtUtc)
            .HasPrecision(0);

        builder.Property(u => u.FailedLoginAttempts)
            .IsRequired();

        builder.Property(u => u.LockoutEndUtc)
            .HasPrecision(0);

        builder.Property(u => u.LastLoginAt)
            .HasPrecision(0);

        builder.Property(u => u.Dni)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(u => u.Dni)
            .IsUnique();
    }
}
