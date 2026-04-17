using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using BrownieShopAPI.Models;

namespace BrownieShopAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase {
    private readonly AppDbContext _db;
    public UsersController(AppDbContext db) { _db = db; }

    [HttpPost("signup")]
    public async Task<IActionResult> Signup(SignupDto dto) {
        if(_db.Users.Any(u => u.Email == dto.Email))
            return BadRequest("Email already exists");
        var user = new User {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Phone = dto.Phone,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return Ok(user);
    }

    [HttpPost("login")]
    public IActionResult Login(LoginDto dto) {
        var user = _db.Users.FirstOrDefault(u => u.Email == dto.EmailOrPhone
                                              || u.Phone == dto.EmailOrPhone);
        if(user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return Unauthorized("Invalid credentials");
        return Ok(user);
    }
}
