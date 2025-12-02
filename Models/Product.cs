namespace ConsoleApp1.Models;

public class Product
{
    public int ProductId { get; set; }  // PK
    public string Name { get; set; }
    public decimal Price { get; set; }

    // FK
    public int CategoryId { get; set; }

    // Navigation
    public Category Category { get; set; }
    public List<OrderRow> OrderRows { get; set; } = new();
}