using Microsoft.EntityFrameworkCore;
using ConsoleApp1.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsoleApp1.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.CustomerID);
        builder.Property(c => c.Name).IsRequired();
        builder.Property(c => c.Email).IsRequired();
    }
}