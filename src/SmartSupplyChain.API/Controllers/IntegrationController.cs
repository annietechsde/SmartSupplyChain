
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartSupplyChain.Application.Interfaces;
using SmartSupplyChain.Core.DTOs.Orders;

namespace SmartSupplyChain.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class IntegrationController : ControllerBase
{
    private readonly IOrderService _orderService;

    public IntegrationController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok(new
        {
            message = "SmartSupplyChain Integration API is working."
        });
    }

    [HttpPost("orders")]
    public async Task<IActionResult> CreateExternalOrder(
        [FromBody] ExternalOrderDto request)
    {
        var created = await _orderService.CreateExternalOrderAsync(
            request);

        if (!created)
        {
            return BadRequest(new
            {
                message = "The external order could not be created. Check the order details and ensure the external order ID has not already been used."
            });
        }

        return Ok(new
        {
            message = "External order created successfully."
        });
    }
}
