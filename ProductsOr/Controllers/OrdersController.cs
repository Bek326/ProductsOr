using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsOr.Data;
using ProductsOr.Models;

namespace ProductsOr.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly AppDbContext _context;

    public OrdersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Order>> Get()
    {
        return _context.Orders.Include(o => o.Products).ToList();
    }

    [HttpPost]
    public ActionResult<Order> Post(Order order)
    {
        // Убедитесь, что Id не задается вручную
        foreach (var product in order.Products)
        {
            product.Id = 0; // Сбросьте Id, чтобы избежать конфликтов
        }

        _context.Orders.Add(order);
        _context.SaveChanges();

        return CreatedAtAction(nameof(Get), new { id = order.Id }, order);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var order = _context.Orders.Include(o => o.Products).FirstOrDefault(o => o.Id == id);
        if (order == null)
        {
            return NotFound();
        }

        _context.Orders.Remove(order);
        _context.SaveChanges();

        return NoContent();
    }
}