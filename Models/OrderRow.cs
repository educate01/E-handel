namespace ConsoleApp1.Models;

public class OrderRow
{
    public int OrderRowId { get; set; }   // PK

    public int Quantity { get; set; }
    public decimal PriceAtPurchase { get; set; }

    // FK
    public int OrderId { get; set; }
    public int ProductId { get; set; }

    // Navigation
    public Order Order { get; set; }
    public Product Product { get; set; }
}