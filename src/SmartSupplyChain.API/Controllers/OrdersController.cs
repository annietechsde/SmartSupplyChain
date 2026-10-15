
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartSupplyChain.Application.Interfaces;
using SmartSupplyChain.Core.DTOs.Orders;

namespace SmartSupplyChain.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        int? userId = GetUserId();

        if (User.IsInRole("Admin"))
        {
            userId = null;
        }
        else if (userId is null)
        {
            return Forbid();
        }

        var orders = await _orderService.GetAllAsync(userId);
        return Ok(orders);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        int? userId = GetUserId();

        if (User.IsInRole("Admin"))
        {
            userId = null;
        }
        else if (userId is null)
        {
            return Forbid();
        }

        var order = await _orderService.GetByIdAsync(id, userId);

        if (order is null)
        {
            return NotFound(new { Message = "Order not found." });
        }

        return Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateOrderDto request)
    {
        int? userId = GetUserId();

        if (User.IsInRole("Admin"))
        {
            userId = null;
        }
        else if (userId is null)
        {
            return Forbid();
        }

        var created = await _orderService.CreateAsync(request, userId);

        if (!created)
        {
            return BadRequest(new { Message = "Unable to create order." });
        }

        return Ok(new { Message = "Order created successfully." });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        int? userId = GetUserId();

        if (User.IsInRole("Admin"))
        {
            userId = null;
        }
        else if (userId is null)
        {
            return Forbid();
        }

        var deleted = await _orderService.DeleteAsync(id, userId);

        if (!deleted)
        {
            return NotFound(new { Message = "Order not found." });
        }

        return Ok(new { Message = "Order deleted successfully." });
    }

    private int? GetUserId()
    {
        var userIdClaim =
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (int.TryParse(userIdClaim, out int userId) && userId > 0)
        {
            return userId;
        }

        return null;
    }
}
