using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartSupplyChain.Application.Services;

namespace SmartSupplyChain.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TrackingController : ControllerBase
{
    private readonly TrackingService _trackingService;

    public TrackingController(TrackingService trackingService)
    {
        _trackingService = trackingService;
    }

    [HttpGet("{trackingNumber}")]
    public async Task<IActionResult> GetTracking(
        string trackingNumber)
    {
        var tracking = await _trackingService
            .GetTrackingAsync(trackingNumber);

        if (tracking is null)
        {
            return NotFound(new
            {
                Message = "Shipment tracking information not found."
            });
        }

        return Ok(tracking);
    }
}