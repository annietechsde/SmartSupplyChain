using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartSupplyChain.Application.Services;

namespace SmartSupplyChain.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportsController : ControllerBase
{
    private readonly ReportService _reportService;

    public ReportsController(ReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("orders")]
    public async Task<IActionResult> GetOrderReport()
    {
        var report = await _reportService.GetOrderReportAsync();

        return Ok(report);
    }

    [HttpGet("shipments")]
    public async Task<IActionResult> GetShipmentReport()
    {
        var report = await _reportService.GetShipmentReportAsync();

        return Ok(report);
    }

    [HttpGet("payments")]
    public async Task<IActionResult> GetPaymentReport()
    {
        var report = await _reportService.GetPaymentReportAsync();

        return Ok(report);
    }

    [HttpGet("deliveries")]
    public async Task<IActionResult> GetDeliveryReport()
    {
        var report = await _reportService.GetDeliveryReportAsync();

        return Ok(report);
    }
}