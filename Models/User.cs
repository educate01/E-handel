// Models/User.cs
namespace ConsoleApp1.Models;

public class User
{
    public int UserId { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }  // Hash + salt kommer lagras här
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}