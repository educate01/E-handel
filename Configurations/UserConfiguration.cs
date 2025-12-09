// Configurations/UserConfiguration.cs
using ConsoleApp1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsoleApp1.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.UserId);
        builder.Property(u => u.Username).IsRequired().HasMaxLength(50);
        builder.Property(u => u.PasswordHash).IsRequired();
        builder.HasIndex(u => u.Username).IsUnique();  // Användarnamn måste vara unikt
    }
}