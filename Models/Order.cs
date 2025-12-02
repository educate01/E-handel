namespace ConsoleApp1.Models;

public class Order
{
    public int OrderId { get; set; }     // PK
    public DateTime OrderDate { get; set; }

    // FK
    public int CustomerId { get; set; }

    // Navigation
    public Customer Customer { get; set; }
    public List<OrderRow> OrderRows { get; set; } = new();
}