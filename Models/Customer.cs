namespace ConsoleApp1.Models;

public class Customer
{
    public int CustomerID { get; set; } //PK
    public string Name { get; set; }
    public string Email { get; set; }
    
    //navigation
    public List<Order> Orders { get; set; } = new();
}