using ConsoleApp1.Data;
using ConsoleApp1.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp1.Services;

public class OrderRowService
{
    private readonly ShopContext _context;

    public OrderRowService(ShopContext context)
    {
        _context = context;
    }

    // CREATE
    public async Task AddOrderRowAsync(OrderRow orderRow)
    {
        _context.OrderRows.Add(orderRow);
        await _context.SaveChangesAsync();
    }

    // READ ALL
    public async Task<List<OrderRow>> GetAllAsync()
    {
        return await _context.OrderRows
            .Include(or => or.Product)
            .Include(or => or.Order)
            .ToListAsync();
    }

    // READ ONE
    public async Task<OrderRow?> GetByIdAsync(int id)
    {
        return await _context.OrderRows
            .Include(or => or.Product)
            .Include(or => or.Order)
            .FirstOrDefaultAsync(or => or.OrderRowId == id);
    }

    // UPDATE
    public async Task UpdateAsync(OrderRow orderRow)
    {
        _context.OrderRows.Update(orderRow);
        await _context.SaveChangesAsync();
    }

    // DELETE
    public async Task DeleteAsync(int id)
    {
        var row = await _context.OrderRows.FindAsync(id);
        if (row != null)
        {
            _context.OrderRows.Remove(row);
            await _context.SaveChangesAsync();
        }
    }
}