using ConsoleApp1.Data;
using ConsoleApp1.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp1.Services;

public class CustomerService
{

    public CustomerService()
    {
        
    }

    // CREATE
    public async Task AddCustomerAsync(Customer customer)
    {
        using var context = new ShopContext();
        context.Customers.Add(customer);
        await context.SaveChangesAsync();
    }

    // READ ALL
    public async Task<List<Customer>> GetAllAsync()
    {
        using var context = new ShopContext();
        return await context.Customers.ToListAsync();
    }

    // READ ONE
    public async Task<Customer?> GetByIdAsync(int id)
    {
        using var  context = new ShopContext();
        return await context.Customers
            .Include(c => c.Orders)
            .FirstOrDefaultAsync(c => c.CustomerID == id);
    }

    // UPDATE
    public async Task UpdateAsync(Customer customer)
    {
        using var context = new ShopContext();
        context.Customers.Update(customer);
        await context.SaveChangesAsync();
    }

    // DELETE
    public async Task DeleteAsync(int id)
    {
        using var  context = new ShopContext();
        var customer = await context.Customers.FindAsync(id);
        if (customer != null)
        {
            context.Customers.Remove(customer);
            await context.SaveChangesAsync();
        }
    }
    public async Task<List<Customer>> SearchCustomersByNameAsync(string name)
    {
        using var context = new ShopContext();
        return await context.Customers
            .Where(c => c.Name.Contains(name))
            .ToListAsync();
    }
}