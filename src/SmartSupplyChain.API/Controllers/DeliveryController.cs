using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartSupplyChain.Application.Interfaces;

namespace SmartSupplyChain.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DeliveryController : ControllerBase
{
    private readonly IDeliveryService _deliveryService;

    public DeliveryController(IDeliveryService deliveryService)
    {
        _deliveryService = deliveryService;
    }

    [HttpPost("assign")]
    public async Task<IActionResult> AssignShipment(
        [FromQuery] int shipmentId,
        [FromQuery] int deliveryAgentId)
    {
        var assigned = await _deliveryService.AssignShipmentAsync(
            shipmentId,
            deliveryAgentId);

        if (!assigned)
        {
            return BadRequest(new
            {
                Message = "Unable to assign shipment."
            });
        }

        return Ok(new
        {
            Message = "Shipment assigned successfully."
        });
    }

    [HttpPut("{assignmentId:int}/accept")]
    public async Task<IActionResult> AcceptAssignment(int assignmentId)
    {
        var accepted = await _deliveryService
            .AcceptAssignmentAsync(assignmentId);

        if (!accepted)
        {
            return NotFound(new
            {
                Message = "Delivery assignment not found."
            });
        }

        return Ok(new
        {
            Message = "Delivery assignment accepted successfully."
        });
    }

    [HttpPut("{assignmentId:int}/complete")]
    public async Task<IActionResult> CompleteDelivery(int assignmentId)
    {
        var completed = await _deliveryService
            .CompleteDeliveryAsync(assignmentId);

        if (!completed)
        {
            return NotFound(new
            {
                Message = "Delivery assignment not found."
            });
        }

        return Ok(new
        {
            Message = "Delivery completed successfully."
        });
    }

    [HttpPut("{assignmentId:int}/status")]
    public async Task<IActionResult> UpdateStatus(
        int assignmentId,
        [FromQuery] string status)
    {
        var updated = await _deliveryService
            .UpdateDeliveryStatusAsync(assignmentId, status);

        if (!updated)
        {
            return BadRequest(new
            {
                Message = "Unable to update delivery status."
            });
        }

        return Ok(new
        {
            Message = "Delivery status updated successfully."
        });
    }

    [HttpGet("agent/{deliveryAgentId:int}")]
    public async Task<IActionResult> GetAgentAssignments(
        int deliveryAgentId)
    {
        var assignments = await _deliveryService
            .GetAgentAssignmentsAsync(deliveryAgentId);

        return Ok(assignments);
    }
}