using ConsoleApp1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsoleApp1.Configurations;

public class OrderRowConfiguration : IEntityTypeConfiguration<OrderRow>
{
    public void Configure(EntityTypeBuilder<OrderRow> builder)
    {
        builder.HasKey(or => or.OrderRowId);

        builder.HasOne(or => or.Order)
            .WithMany(o => o.OrderRows)
            .HasForeignKey(or => or.OrderId);

        builder.HasOne(or => or.Product)
            .WithMany(p => p.OrderRows)
            .HasForeignKey(or => or.ProductId);
    }
}