using ConsoleApp1.Data;
using ConsoleApp1.Models;
using ConsoleApp1.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp1;

class Program
{
    private static ServiceProvider _serviceProvider;
    private static UserService _userService;
    private static CustomerService _customerService;
    private static ProductService _productService;
    private static CategoryService _categoryService;
    private static OrderService _orderService;
    private static OrderRowService _orderRowService;
    private static User _currentUser;

    static async Task Main(string[] args)
    {
        InitializeServices();
        
        Console.WriteLine("=== SHOP MANAGEMENT SYSTEM ===");
        
        // Login eller registrera
        if (!await HandleAuthentication())
        {
            Console.WriteLine("Authentication failed. Exiting...");
            return;
        }
        
        // Huvudmeny
        await MainMenu();
        
        Console.WriteLine("\nThank you for using Shop Management System!");
    }
    
    static void InitializeServices()
    {
        var services = new ServiceCollection();
        services.AddDbContext<ShopContext>();
        services.AddScoped<CustomerService>();
        services.AddScoped<ProductService>();
        services.AddScoped<CategoryService>();
        services.AddScoped<OrderService>();
        services.AddScoped<OrderRowService>();
        services.AddScoped<UserService>();
        
        _serviceProvider = services.BuildServiceProvider();
        
        using var scope = _serviceProvider.CreateScope();
        _userService = scope.ServiceProvider.GetRequiredService<UserService>();
        _customerService = scope.ServiceProvider.GetRequiredService<CustomerService>();
        _productService = scope.ServiceProvider.GetRequiredService<ProductService>();
        _categoryService = scope.ServiceProvider.GetRequiredService<CategoryService>();
        _orderService = scope.ServiceProvider.GetRequiredService<OrderService>();
        _orderRowService = scope.ServiceProvider.GetRequiredService<OrderRowService>();
    }
    
    static async Task<bool> HandleAuthentication()
    {
        Console.WriteLine("\n=== AUTHENTICATION ===");
        Console.WriteLine("1. Login");
        Console.WriteLine("2. Register");
        Console.Write("Choose option: ");
        
        var choice = Console.ReadLine();
        
        if (choice == "1")
        {
            return await Login();
        }
        else if (choice == "2")
        {
            return await Register();
        }
        
        return false;
    }
    
    static async Task<bool> Login()
    {
        Console.Write("\nUsername: ");
        var username = Console.ReadLine();
        
        Console.Write("Password: ");
        var password = ReadPassword();
        
        try
        {
            _currentUser = await _userService.LoginAsync(username, password);
            if (_currentUser != null)
            {
                Console.WriteLine($"\n✅ Welcome, {_currentUser.Username}!");
                return true;
            }
            Console.WriteLine("\n❌ Invalid username or password");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n⚠️ Error: {ex.Message}");
            return false;
        }
    }
    
    static async Task<bool> Register()
    {
        Console.Write("\nChoose username: ");
        var username = Console.ReadLine();
        
        Console.Write("Choose password: ");
        var password = ReadPassword();
        
        try
        {
            await _userService.RegisterUserAsync(username, password);
            Console.WriteLine($"\n✅ User '{username}' registered successfully!");
            Console.WriteLine("Please login with your new credentials.");
            return await Login();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n❌ Registration failed: {ex.Message}");
            return false;
        }
    }
    
    static async Task MainMenu()
    {
        bool exit = false;
        
        while (!exit)
        {
            Console.Clear();
            Console.WriteLine($"=== MAIN MENU (Logged in as: {_currentUser?.Username}) ===");
            Console.WriteLine("1. Customer Management");
            Console.WriteLine("2. Product Management");
            Console.WriteLine("3. Category Management");
            Console.WriteLine("4. Order Management");
            Console.WriteLine("5. Reports");
            Console.WriteLine("6. Logout");
            Console.WriteLine("7. Exit");
            Console.Write("\nChoose option: ");
            
            var choice = Console.ReadLine();
            
            switch (choice)
            {
                case "1":
                    await CustomerMenu();
                    break;
                case "2":
                    await ProductMenu();
                    break;
                case "3":
                    await CategoryMenu();
                    break;
                case "4":
                    await OrderMenu();
                    break;
                case "5":
                    await ReportsMenu();
                    break;
                case "6":
                    _currentUser = null;
                    Console.WriteLine("Logged out!");
                    Console.ReadKey();
                    if (!await HandleAuthentication())
                        exit = true;
                    break;
                case "7":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid option!");
                    Console.ReadKey();
                    break;
            }
        }
    }
    
    // === CUSTOMER MENU ===
    static async Task CustomerMenu()
    {
        bool back = false;
        
        while (!back)
        {
            Console.Clear();
            Console.WriteLine("=== CUSTOMER MANAGEMENT ===");
            Console.WriteLine("1. List all customers");
            Console.WriteLine("2. Search customers by name");
            Console.WriteLine("3. Add new customer");
            Console.WriteLine("4. View customer details");
            Console.WriteLine("5. Update customer");
            Console.WriteLine("6. Delete customer");
            Console.WriteLine("7. Back to main menu");
            Console.Write("\nChoose: ");
            
            var choice = Console.ReadLine();
            
            try
            {
                switch (choice)
                {
                    case "1": // LIST ALL
                        var customers = await _customerService.GetAllAsync();
                        Console.WriteLine($"\nTotal customers: {customers.Count}");
                        foreach (var c in customers)
                        {
                            Console.WriteLine($"ID: {c.CustomerID} | Name: {c.Name} | Email: {c.Email}");
                        }
                        WaitForKey();
                        break;
                        
                    case "2": // SEARCH
                        Console.Write("Enter name to search: ");
                        var searchName = Console.ReadLine();
                        var foundCustomers = await _customerService.SearchCustomersByNameAsync(searchName);
                        Console.WriteLine($"\nFound {foundCustomers.Count} customers:");
                        foreach (var c in foundCustomers)
                        {
                            Console.WriteLine($"- {c.Name} ({c.Email})");
                        }
                        WaitForKey();
                        break;
                        
                    case "3": // ADD
                        Console.Write("Enter customer name: ");
                        var name = Console.ReadLine();
                        Console.Write("Enter email: ");
                        var email = Console.ReadLine();
                        
                        var newCustomer = new Customer 
                        { 
                            Name = name, 
                            Email = email 
                        };
                        await _customerService.AddCustomerAsync(newCustomer);
                        Console.WriteLine("\n✅ Customer added successfully!");
                        WaitForKey();
                        break;
                        
                    case "4": // VIEW DETAILS
                        Console.Write("Enter customer ID: ");
                        if (int.TryParse(Console.ReadLine(), out int viewId))
                        {
                            var customer = await _customerService.GetByIdAsync(viewId);
                            if (customer != null)
                            {
                                Console.WriteLine($"\n=== CUSTOMER DETAILS ===");
                                Console.WriteLine($"ID: {customer.CustomerID}");
                                Console.WriteLine($"Name: {customer.Name}");
                                Console.WriteLine($"Email: {customer.Email}");
                                Console.WriteLine($"Orders: {customer.Orders?.Count ?? 0}");
                                
                                if (customer.Orders?.Any() == true)
                                {
                                    Console.WriteLine("\nRecent orders:");
                                    foreach (var order in customer.Orders.Take(5))
                                    {
                                        Console.WriteLine($"- Order #{order.OrderId} on {order.OrderDate:d}");
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("Customer not found!");
                            }
                        }
                        WaitForKey();
                        break;
                        
                    case "7":
                        back = true;
                        break;
                        
                    default:
                        Console.WriteLine("Invalid option!");
                        WaitForKey();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Error: {ex.Message}");
                WaitForKey();
            }
        }
    }
    
    // === PRODUCT MENU ===
    static async Task ProductMenu()
    {
        bool back = false;
        
        while (!back)
        {
            Console.Clear();
            Console.WriteLine("=== PRODUCT MANAGEMENT ===");
            Console.WriteLine("1. List all products");
            Console.WriteLine("2. Search products");
            Console.WriteLine("3. Add new product");
            Console.WriteLine("4. View product details");
            Console.WriteLine("5. Update product price");
            Console.WriteLine("6. Delete product");
            Console.WriteLine("7. Filter by category");
            Console.WriteLine("8. Filter by price range");
            Console.WriteLine("9. Back to main menu");
            Console.Write("\nChoose: ");
            
            var choice = Console.ReadLine();
            
            try
            {
                switch (choice)
                {
                    case "1": // LIST ALL
                        var products = await _productService.GetProductsAsync();
                        Console.WriteLine($"\nTotal products: {products.Count}");
                        Console.WriteLine("ID | Name | Price | Category");
                        Console.WriteLine("---|------|-------|---------");
                        foreach (var p in products)
                        {
                            Console.WriteLine($"{p.ProductId} | {p.Name} | {p.Price:C} | {p.Category?.Name}");
                        }
                        WaitForKey();
                        break;
                        
                    case "2": // SEARCH
                        Console.Write("Enter search term: ");
                        var term = Console.ReadLine();
                        var results = await _productService.SearchProductsAsync(term);
                        Console.WriteLine($"\nFound {results.Count} products:");
                        foreach (var p in results)
                        {
                            Console.WriteLine($"- {p.Name} ({p.Price:C})");
                        }
                        WaitForKey();
                        break;
                        
                    case "3": // ADD
                        Console.Write("Enter product name: ");
                        var name = Console.ReadLine();
                        Console.Write("Enter price: ");
                        if (decimal.TryParse(Console.ReadLine(), out decimal price))
                        {
                            // Show categories
                            var categories = await _categoryService.GetCategoriesAsync();
                            Console.WriteLine("\nAvailable categories:");
                            foreach (var c in categories)
                            {
                                Console.WriteLine($"ID: {c.CategoryId} - {c.Name}");
                            }
                            
                            Console.Write("Enter category ID: ");
                            if (int.TryParse(Console.ReadLine(), out int categoryId))
                            {
                                var product = new Product
                                {
                                    Name = name,
                                    Price = price,
                                    CategoryId = categoryId
                                };
                                await _productService.AddProductAsync(product);
                                Console.WriteLine("\n✅ Product added!");
                            }
                        }
                        WaitForKey();
                        break;
                        
                    case "7": // FILTER BY CATEGORY
                        var allCategories = await _categoryService.GetCategoriesAsync();
                        Console.WriteLine("\nCategories:");
                        foreach (var c in allCategories)
                        {
                            Console.WriteLine($"ID: {c.CategoryId} - {c.Name}");
                        }
                        Console.Write("Enter category ID: ");
                        if (int.TryParse(Console.ReadLine(), out int filterCategoryId))
                        {
                            // Här behöver du lägga till metoden GetProductsByCategoryAsync i ProductService
                            // var categoryProducts = await _productService.GetProductsByCategoryAsync(filterCategoryId);
                            // Console.WriteLine($"\nProducts in category: {categoryProducts.Count}");
                        }
                        WaitForKey();
                        break;
                        
                    case "9":
                        back = true;
                        break;
                        
                    default:
                        Console.WriteLine("Invalid option!");
                        WaitForKey();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Error: {ex.Message}");
                WaitForKey();
            }
        }
    }
    
    // === CATEGORY MENU ===
    static async Task CategoryMenu()
    {
        bool back = false;
        
        while (!back)
        {
            Console.Clear();
            Console.WriteLine("=== CATEGORY MANAGEMENT ===");
            Console.WriteLine("1. List all categories");
            Console.WriteLine("2. Add new category");
            Console.WriteLine("3. View products in category");
            Console.WriteLine("4. Back to main menu");
            Console.Write("\nChoose: ");
            
            var choice = Console.ReadLine();
            
            try
            {
                switch (choice)
                {
                    case "1":
                        var categories = await _categoryService.GetCategoriesAsync();
                        Console.WriteLine($"\nTotal categories: {categories.Count}");
                        foreach (var c in categories)
                        {
                            var products = await _productService.GetProductsAsync();
                            var productCount = products.Count(p => p.CategoryId == c.CategoryId);
                            Console.WriteLine($"- {c.Name} (ID: {c.CategoryId}, Products: {productCount})");
                        }
                        WaitForKey();
                        break;
                        
                    case "2":
                        Console.Write("Enter category name: ");
                        var name = Console.ReadLine();
                        var category = new Category { Name = name };
                        await _categoryService.AddCategoryAsync(category);
                        Console.WriteLine("\n✅ Category added!");
                        WaitForKey();
                        break;
                        
                    case "4":
                        back = true;
                        break;
                        
                    default:
                        Console.WriteLine("Invalid option!");
                        WaitForKey();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Error: {ex.Message}");
                WaitForKey();
            }
        }
    }
    
    // === ORDER MENU ===
    static async Task OrderMenu()
    {
        bool back = false;
        
        while (!back)
        {
            Console.Clear();
            Console.WriteLine("=== ORDER MANAGEMENT ===");
            Console.WriteLine("1. List all orders");
            Console.WriteLine("2. Create new order");
            Console.WriteLine("3. View order details");
            Console.WriteLine("4. Search orders by customer");
            Console.WriteLine("5. Back to main menu");
            Console.Write("\nChoose: ");
            
            var choice = Console.ReadLine();
            
            try
            {
                switch (choice)
                {
                    case "1": // LIST ALL
                        var orders = await _orderService.GetAllAsync();
                        Console.WriteLine($"\nTotal orders: {orders.Count}");
                        foreach (var o in orders)
                        {
                            Console.WriteLine($"Order #{o.OrderId} | Date: {o.OrderDate:d} | Customer: {o.Customer?.Name}");
                            Console.WriteLine($"  Total items: {o.OrderRows?.Count ?? 0}");
                        }
                        WaitForKey();
                        break;
                        
                    case "3": // VIEW DETAILS
                        Console.Write("Enter order ID: ");
                        if (int.TryParse(Console.ReadLine(), out int orderId))
                        {
                            var order = await _orderService.GetByIdAsync(orderId);
                            if (order != null)
                            {
                                Console.WriteLine($"\n=== ORDER #{order.OrderId} ===");
                                Console.WriteLine($"Date: {order.OrderDate}");
                                Console.WriteLine($"Customer: {order.Customer?.Name} ({order.Customer?.Email})");
                                Console.WriteLine("\nOrder Items:");
                                Console.WriteLine("----------------------------------------");
                                decimal total = 0;
                                
                                if (order.OrderRows?.Any() == true)
                                {
                                    foreach (var row in order.OrderRows)
                                    {
                                        var rowTotal = row.Quantity * row.PriceAtPurchase;
                                        total += rowTotal;
                                        Console.WriteLine($"{row.Product?.Name} x {row.Quantity} @ {row.PriceAtPurchase:C} = {rowTotal:C}");
                                    }
                                }
                                Console.WriteLine("----------------------------------------");
                                Console.WriteLine($"TOTAL: {total:C}");
                            }
                            else
                            {
                                Console.WriteLine("Order not found!");
                            }
                        }
                        WaitForKey();
                        break;
                        
                    case "4": // SEARCH BY CUSTOMER
                        Console.Write("Enter customer ID: ");
                        if (int.TryParse(Console.ReadLine(), out int customerId))
                        {
                            // Här behöver du lägga till GetOrdersByCustomerAsync i OrderService
                            // var customerOrders = await _orderService.GetOrdersByCustomerAsync(customerId);
                            // Console.WriteLine($"\nOrders for customer: {customerOrders.Count}");
                        }
                        WaitForKey();
                        break;
                        
                    case "5":
                        back = true;
                        break;
                        
                    default:
                        Console.WriteLine("Invalid option!");
                        WaitForKey();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Error: {ex.Message}");
                WaitForKey();
            }
        }
    }
    
    // === REPORTS MENU ===
    static async Task ReportsMenu()
    {
        bool back = false;
        
        while (!back)
        {
            Console.Clear();
            Console.WriteLine("=== REPORTS ===");
            Console.WriteLine("1. Sales summary");
            Console.WriteLine("2. Top products");
            Console.WriteLine("3. Customer statistics");
            Console.WriteLine("4. Back to main menu");
            Console.Write("\nChoose: ");
            
            var choice = Console.ReadLine();
            
            try
            {
                switch (choice)
                {
                    case "1": // SALES SUMMARY
                        var orders = await _orderService.GetAllAsync();
                        decimal totalSales = 0;
                        int totalItems = 0;
                        
                        foreach (var order in orders)
                        {
                            if (order.OrderRows != null)
                            {
                                foreach (var row in order.OrderRows)
                                {
                                    totalSales += row.Quantity * row.PriceAtPurchase;
                                    totalItems += row.Quantity;
                                }
                            }
                        }
                        
                        Console.WriteLine($"\n=== SALES SUMMARY ===");
                        Console.WriteLine($"Total orders: {orders.Count}");
                        Console.WriteLine($"Total items sold: {totalItems}");
                        Console.WriteLine($"Total revenue: {totalSales:C}");
                        WaitForKey();
                        break;
                        
                    case "4":
                        back = true;
                        break;
                        
                    default:
                        Console.WriteLine("Invalid option!");
                        WaitForKey();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Error: {ex.Message}");
                WaitForKey();
            }
        }
    }
    
    // === HJÄLPMETODER ===
    static string ReadPassword()
    {
        var password = "";
        ConsoleKeyInfo key;
        
        do
        {
            key = Console.ReadKey(true);
            
            if (key.Key != ConsoleKey.Backspace && 
                key.Key != ConsoleKey.Enter)
            {
                password += key.KeyChar;
                Console.Write("*");
            }
            else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password.Remove(password.Length - 1);
                Console.Write("\b \b");
            }
        } while (key.Key != ConsoleKey.Enter);
        
        Console.WriteLine();
        return password;
    }
    
    static void WaitForKey()
    {
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }
}



