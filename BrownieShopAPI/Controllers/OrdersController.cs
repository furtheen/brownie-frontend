using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using BrownieShopAPI.Models;
using Razorpay.Api;

namespace BrownieShopAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase {
    private readonly AppDbContext _db;
    public OrdersController(AppDbContext db) { _db = db; }

    // Step 1: Create Razorpay order
    [HttpPost("create-razorpay-order")]
    public IActionResult CreateRazorpayOrder([FromBody] int amount) {
        var client = new RazorpayClient("YOUR_KEY_ID", "YOUR_KEY_SECRET");
        var options = new Dictionary<string, object> {
            { "amount", amount * 100 }, // paise
            { "currency", "INR" },
            { "receipt", "rcpt_" + DateTime.Now.Ticks }
        };
        var order = client.Order.Create(options);
        return Ok(new { orderId = order["id"].ToString() });
    }

    // Step 2: Verify payment & save order
    [HttpPost("verify-and-save")]
    public async Task<IActionResult> VerifyAndSave(OrderDto dto) {
        try {
            // Verify signature from Razorpay
            var attributes = new Dictionary<string, string> {
                { "razorpay_order_id", dto.RazorpayOrderId },
                { "razorpay_payment_id", dto.RazorpayPaymentId },
                { "razorpay_signature", dto.RazorpaySignature }
            };
            
            Utils.verifyPaymentSignature(attributes); // throws if fake

            var order = new BrownieShopAPI.Models.Order {
                CustomerName = dto.CustomerName,
                Phone = dto.Phone,
                Address = dto.Address,
                Items = dto.Items,
                Amount = dto.Amount,
                PaymentStatus = "PAID",
                RazorpayPaymentId = dto.RazorpayPaymentId
            };
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();
            return Ok(order);
        } catch (Exception ex) {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet]
    public IActionResult GetOrders() => Ok(_db.Orders.OrderByDescending(o => o.CreatedAt).ToList());
}
