using ConsoleApp1.Data;
using ConsoleApp1.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp1.Services;

public class ProductService
{

    public ProductService()
    {
        
    }

    public async Task AddProductAsync(Product product)
    {
        using var  context = new ShopContext();
        context.Products.Add(product);
        await context.SaveChangesAsync();
    }

    public async Task<List<Product>> GetProductsAsync()
    {
        using var context = new ShopContext();
        return await context.Products.Include(p => p.Category).ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        using var context = new ShopContext();
        return await context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.ProductId == id);
    }

    public async Task UpdateProductAsync(Product product)
    {
        using var  context = new ShopContext();
        context.Products.Update(product);
        await context.SaveChangesAsync();
    }

    public async Task DeleteProductAsync(int id)
    {
        using var  context = new ShopContext();
        var product = await context.Products.FindAsync(id);
        if (product != null)
        {
            context.Products.Remove(product);
            await context.SaveChangesAsync();
        }
    }
    public async Task<List<Product>> SearchProductsAsync(string searchTerm)
    {
        using var context = new ShopContext();
        return await context.Products
            .Where(p => p.Name.Contains(searchTerm))
            .Include(p => p.Category)
            .ToListAsync();
    }
    public async Task<List<Product>> GetProductsByCategoryAsync(int categoryId)
    {
        using var  context = new ShopContext();
        return await context.Products
            .Include(p => p.Category)
            .Where(p => p.CategoryId == categoryId)
            .ToListAsync();
    }

    public async Task<List<Product>> GetProductsByPriceRangeAsync(decimal min, decimal max)
    {
        using var  context = new ShopContext();
        return await context.Products
            .Include(p => p.Category)
            .Where(p => p.Price >= min && p.Price <= max)
            .OrderBy(p => p.Price)
            .ToListAsync();
    }
}

