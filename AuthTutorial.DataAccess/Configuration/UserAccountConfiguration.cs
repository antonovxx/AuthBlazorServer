using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configuration;

public class UserAccountConfiguration : IEntityTypeConfiguration<UserAccount>
{
    public void Configure(EntityTypeBuilder<UserAccount> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.CreatedAtUtc)
            .HasColumnName("created_at")
            .HasConversion<DateTime>()
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.UpdatedAtUtc)
            .HasColumnName("updated_at")
            .HasConversion<DateTime>()
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.Name)
            .HasColumnName("user_name")
            .HasConversion<string>()
            .IsRequired();

        builder.Property(x => x.Role)
            .HasColumnName("role")
            .HasConversion<int>()
            .IsRequired()
            .HasDefaultValue(RoleType.None);

        builder.Property(x => x.Password)
            .HasColumnName("password")
            .HasConversion<string>()
            .IsRequired();

        builder.HasData(
            new UserAccount
            {
                Id = 1,
                Name = "admin",
                Password = "Test123*",
                Role = RoleType.Administrator,
            });
        
        builder.HasData(
            new UserAccount
            {
                Id = 2,
                Name = "user",
                Password = "Test123*",
                Role = RoleType.User,
            });
    }
}