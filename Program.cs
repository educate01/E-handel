using ConsoleApp1.Data;
using ConsoleApp1.Services;
using ConsoleApp1.Models;

var db = new ShopContext();
var productService = new ProductService(db);

await productService.AddProductAsync(new Product
{
    Name = "Testprodukt",
    Price = 999,
    CategoryId = 1
});

var products = await productService.GetProductsAsync();
foreach (var p in products)
{
    Console.WriteLine($"{p.ProductId} - {p.Name} ({p.Price} kr)");
}