// Services/UserService.cs
using ConsoleApp1.Data;
using ConsoleApp1.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace ConsoleApp1.Services;

public class UserService
{

    public UserService()
    {
    }

    // Skapa en hash av lösenordet med salt
    public string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        
        // Skapa ett random salt
        var saltBytes = new byte[16];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(saltBytes);
        var salt = Convert.ToBase64String(saltBytes);
        
        // Kombinera salt + lösenord
        var passwordWithSalt = salt + password;
        var hashBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(passwordWithSalt));
        
        // Spara både salt och hash tillsammans (salt:hash)
        return $"{salt}:{Convert.ToBase64String(hashBytes)}";
    }

    // Verifiera lösenord
    public bool VerifyPassword(string password, string storedHash)
    {
        var parts = storedHash.Split(':');
        if (parts.Length != 2) return false;
        
        var salt = parts[0];
        var expectedHash = parts[1];
        
        using var sha256 = SHA256.Create();
        var passwordWithSalt = salt + password;
        var hashBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(passwordWithSalt));
        var actualHash = Convert.ToBase64String(hashBytes);
        
        return actualHash == expectedHash;
    }

    // CREATE - Registrera ny användare
    public async Task RegisterUserAsync(string username, string password)
    {
        using var context = new ShopContext();
        // Kontrollera om användarnamn redan finns
        if (await context.Users.AnyAsync(u => u.Username == username))
            throw new Exception("Username already exists");
        
        var hashedPassword = HashPassword(password);
        
        var user = new User
        {
            Username = username,
            PasswordHash = hashedPassword
        };
        
        context.Users.Add(user);
        await context.SaveChangesAsync();
    }

    // LOGIN - Verifiera användare
    public async Task<User?> LoginAsync(string username, string password)
    {
        using var context = new ShopContext();
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Username == username);
        
        if (user == null) return null;
        
        if (VerifyPassword(password, user.PasswordHash))
            return user;
        
        return null;
    }

    // Andra CRUD-metoder...
    public async Task<List<User>> GetAllUsersAsync()
    {
        using var  context = new ShopContext();
        return await context.Users.ToListAsync();
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        using var context = new ShopContext();
        return await context.Users.FindAsync(id);
    }
}