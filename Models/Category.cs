namespace ConsoleApp1.Models;

public class Category
{
    public int CategoryId { get; set; } // PK
    public string Name { get; set; }

    // Navigation
    public List<Product> Products { get; set; } = new();
}