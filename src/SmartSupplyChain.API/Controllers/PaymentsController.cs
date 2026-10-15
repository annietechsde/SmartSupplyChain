using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartSupplyChain.Application.Services;
using SmartSupplyChain.Core.Enums;

namespace SmartSupplyChain.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PaymentsController : ControllerBase
{
    private readonly PaymentService _paymentService;

    public PaymentsController(PaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePayment(
        [FromQuery] int orderId,
        [FromQuery] decimal amount,
        [FromQuery] PaymentMethod method)
    {
        var created = await _paymentService.CreatePaymentAsync(
            orderId,
            amount,
            method);

        if (!created)
        {
            return BadRequest(new
            {
                Message = "Unable to create payment."
            });
        }

        return Ok(new
        {
            Message = "Payment created successfully."
        });
    }

    [HttpPut("{orderId:int}/status")]
    public async Task<IActionResult> UpdatePaymentStatus(
        int orderId,
        [FromQuery] PaymentStatus status,
        [FromQuery] string? transactionId = null)
    {
        var updated = await _paymentService.UpdatePaymentStatusAsync(
            orderId,
            status,
            transactionId);

        if (!updated)
        {
            return NotFound(new
            {
                Message = "Payment not found."
            });
        }

        return Ok(new
        {
            Message = "Payment status updated successfully."
        });
    }

    [HttpGet("order/{orderId:int}")]
    public async Task<IActionResult> GetPaymentByOrderId(int orderId)
    {
        var payment = await _paymentService
            .GetPaymentByOrderIdAsync(orderId);

        if (payment is null)
        {
            return NotFound(new
            {
                Message = "Payment not found."
            });
        }

        return Ok(payment);
    }
}