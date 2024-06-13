namespace ProductsOr.Models;

public class Order
{
    public int Id { get; set; }
    public List<Product>? Products { get; set; }
    public decimal TotalAmount => Products.Sum(p => p.Price);
}