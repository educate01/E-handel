using Microsoft.EntityFrameworkCore;
using ConsoleApp1.Models;
using ConsoleApp1.Configurations;
namespace ConsoleApp1.Data;

public class ShopContext : DbContext
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderRow> OrderRows => Set<OrderRow>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite("Data Source=shop.db");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CustomerConfiguration());
        modelBuilder.ApplyConfiguration(new OrderConfiguration());
        modelBuilder.ApplyConfiguration(new OrderRowConfiguration());
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        
        // SEED DATA
        modelBuilder.Entity<Category>().HasData(
            new Category { CategoryId = 1, Name = "Elektronik" },
            new Category { CategoryId = 2, Name = "Kläder" }
        );

        modelBuilder.Entity<Product>().HasData(
            new Product { ProductId = 1, Name = "Laptop", Price = 12000, CategoryId = 1 },
            new Product { ProductId = 2, Name = "T-shirt", Price = 199, CategoryId = 2 }
        );
    }
}

