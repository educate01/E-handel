using ConsoleApp1.Data;
using ConsoleApp1.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp1.Services;

public class OrderRowService
{

    public OrderRowService()
    {
    }

    // CREATE
    public async Task AddOrderRowAsync(OrderRow orderRow)
    {
        using var context = new ShopContext();
        context.OrderRows.Add(orderRow);
        await context.SaveChangesAsync();
    }

    // READ ALL
    public async Task<List<OrderRow>> GetAllAsync()
    {
        using var context = new ShopContext();
        return await context.OrderRows
            .Include(or => or.Product)
            .Include(or => or.Order)
            .ToListAsync();
    }

    // READ ONE
    public async Task<OrderRow?> GetByIdAsync(int id)
    {
        using var context = new ShopContext();
        return await context.OrderRows
            .Include(or => or.Product)
            .Include(or => or.Order)
            .FirstOrDefaultAsync(or => or.OrderRowId == id);
    }

    // UPDATE
    public async Task UpdateAsync(OrderRow orderRow)
    {
        using var context = new ShopContext();
        context.OrderRows.Update(orderRow);
        await context.SaveChangesAsync();
    }

    // DELETE
    public async Task DeleteAsync(int id)
    {
        using var context = new ShopContext();
        var row = await context.OrderRows.FindAsync(id);
        if (row != null)
        {
            context.OrderRows.Remove(row);
            await context.SaveChangesAsync();
        }
    }
}