
using ConsoleApp1.Data;
using ConsoleApp1.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp1.Services;

public class CategoryService
{

    public CategoryService()
    {
        
    }

    // CREATE
    public async Task AddCategoryAsync(Category category)
    {
        using var context = new ShopContext();
        context.Categories.Add(category);
        await context.SaveChangesAsync();
    }

    // READ (alla)
    public async Task<List<Category>> GetCategoriesAsync()
    {
        using var context = new ShopContext();
        return await context.Categories.ToListAsync();
    }

    // READ (en)
    public async Task<Category?> GetCategoryByIdAsync(int id)
    {
        using var context = new ShopContext();
        return await context.Categories
            .FirstOrDefaultAsync(c => c.CategoryId == id);
    }

    // UPDATE
    public async Task UpdateCategoryAsync(Category category)
    {
        using var context = new ShopContext();
        context.Categories.Update(category);
        await context.SaveChangesAsync();
    }

    // DELETE
    public async Task DeleteCategoryAsync(int id)
    {
        using var  context = new ShopContext();
        var category = await context.Categories.FindAsync(id);
        if (category != null)
        {
            context.Categories.Remove(category);
            await context.SaveChangesAsync();
        }
    }
}
