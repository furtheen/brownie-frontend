using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace BrownieShopAPI.Models;

public class User {
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string PasswordHash { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class Product {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Emoji { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; } = true;
}

public class Order {
    public int Id { get; set; }
    public int UserId { get; set; }
    public string CustomerName { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string Items { get; set; }
    public decimal Amount { get; set; }
    public string PaymentStatus { get; set; }
    public string RazorpayPaymentId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class AppDbContext : DbContext {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
}

public class SignupDto {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Password { get; set; }
}

public class LoginDto {
    public string EmailOrPhone { get; set; }
    public string Password { get; set; }
}

public class OrderDto {
    public string CustomerName { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string Items { get; set; }
    public decimal Amount { get; set; }
    public string RazorpayOrderId { get; set; }
    public string RazorpayPaymentId { get; set; }
    public string RazorpaySignature { get; set; }
}
