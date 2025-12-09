using ConsoleApp1.Data;
using ConsoleApp1.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp1.Services;

public class OrderService
{

    public OrderService()
    {
    }

    // CREATE
    public async Task AddOrderAsync(Order order)
    {
        using var context = new ShopContext();
        context.Orders.Add(order);
        await context.SaveChangesAsync();
    }

    // READ ALL
    public async Task<List<Order>> GetAllAsync()
    {
        using var context = new ShopContext();
        return await context.Orders
            .Include(o => o.Customer)
            .Include(o => o.OrderRows)
            .ToListAsync();
    }

    // READ ONE
    public async Task<Order?> GetByIdAsync(int id)
    {
        using var context = new ShopContext();
        return await context.Orders
            .Include(o => o.Customer)
            .Include(o => o.OrderRows)
            .ThenInclude(or => or.Product)
            .FirstOrDefaultAsync(o => o.OrderId == id);
    }

    // UPDATE
    public async Task UpdateAsync(Order order)
    {
        using var context = new ShopContext();
        context.Orders.Update(order);
        await context.SaveChangesAsync();
    }

    // DELETE
    public async Task DeleteAsync(int id)
    {
        using var context = new ShopContext();
        var order = await context.Orders.FindAsync(id);
        if (order != null)
        {
            context.Orders.Remove(order);
            await context.SaveChangesAsync();
        }
    }
    public async Task<List<Order>> GetOrdersByCustomerAsync(int customerId)
    {
        using var context = new ShopContext();
        return await context.Orders
            .Include(o => o.Customer)
            .Include(o => o.OrderRows)
            .Where(o => o.CustomerId == customerId)
            .ToListAsync();
    }
    
}