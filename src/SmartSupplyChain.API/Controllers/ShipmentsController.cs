using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartSupplyChain.Application.Interfaces;
using SmartSupplyChain.Core.DTOs.Shipments;

namespace SmartSupplyChain.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ShipmentsController : ControllerBase
{
    private readonly IShipmentService _shipmentService;

    public ShipmentsController(IShipmentService shipmentService)
    {
        _shipmentService = shipmentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var shipments = await _shipmentService.GetAllAsync();

        return Ok(shipments);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var shipment = await _shipmentService.GetByIdAsync(id);

        if (shipment is null)
        {
            return NotFound(new
            {
                Message = "Shipment not found."
            });
        }

        return Ok(shipment);
    }

    [HttpGet("tracking/{trackingNumber}")]
    public async Task<IActionResult> GetByTrackingNumber(
        string trackingNumber)
    {
        var shipment = await _shipmentService
            .GetByTrackingNumberAsync(trackingNumber);

        if (shipment is null)
        {
            return NotFound(new
            {
                Message = "Shipment not found."
            });
        }

        return Ok(shipment);
    }

    [HttpGet("order/{orderId:int}")]
    public async Task<IActionResult> GetByOrderId(int orderId)
    {
        var shipments = await _shipmentService
            .GetByOrderIdAsync(orderId);

        return Ok(shipments);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateShipmentDto request)
    {
        var created = await _shipmentService.CreateAsync(request);

        if (!created)
        {
            return BadRequest(new
            {
                Message = "Unable to create shipment."
            });
        }

        return Ok(new
        {
            Message = "Shipment created successfully."
        });
    }

    [HttpPut("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(
        int id,
        [FromBody] UpdateShipmentStatusDto request)
    {
        var updated = await _shipmentService
            .UpdateStatusAsync(id, request);

        if (!updated)
        {
            return BadRequest(new
            {
                Message = "Unable to update shipment status."
            });
        }

        return Ok(new
        {
            Message = "Shipment status updated successfully."
        });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _shipmentService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(new
            {
                Message = "Shipment not found."
            });
        }

        return Ok(new
        {
            Message = "Shipment deleted successfully."
        });
    }
}