using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartSupplyChain.Application.Services;
using SmartSupplyChain.Core.Entities;

namespace SmartSupplyChain.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WarehousesController : ControllerBase
{
    private readonly WarehouseService _warehouseService;

    public WarehousesController(WarehouseService warehouseService)
    {
        _warehouseService = warehouseService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var warehouses = await _warehouseService.GetAllAsync();

        return Ok(warehouses);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var warehouse = await _warehouseService.GetByIdAsync(id);

        if (warehouse is null)
        {
            return NotFound(new
            {
                Message = "Warehouse not found."
            });
        }

        return Ok(warehouse);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] Warehouse request)
    {
        var created = await _warehouseService.CreateAsync(request);

        if (!created)
        {
            return BadRequest(new
            {
                Message = "Unable to create warehouse."
            });
        }

        return Ok(new
        {
            Message = "Warehouse created successfully."
        });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] Warehouse request)
    {
        var updated =
            await _warehouseService.UpdateAsync(id, request);

        if (!updated)
        {
            return NotFound(new
            {
                Message = "Warehouse not found."
            });
        }

        return Ok(new
        {
            Message = "Warehouse updated successfully."
        });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted =
            await _warehouseService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(new
            {
                Message = "Warehouse not found."
            });
        }

        return Ok(new
        {
            Message = "Warehouse deleted successfully."
        });
    }
}