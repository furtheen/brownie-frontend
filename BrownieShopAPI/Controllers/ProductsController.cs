using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using BrownieShopAPI.Models;

namespace BrownieShopAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase {
    private readonly AppDbContext _db;
    public ProductsController(AppDbContext db) { _db = db; }

    [HttpGet]
    public IActionResult GetAll() => Ok(_db.Products.ToList());

    // Admin only — toggle availability
    [HttpPatch("{id}/availability")]
    public async Task<IActionResult> Toggle(int id, [FromBody] bool available) {
        var prod = await _db.Products.FindAsync(id);
        if(prod == null) return NotFound();
        prod.IsAvailable = available;
        await _db.SaveChangesAsync();
        return Ok(prod);
    }
}
