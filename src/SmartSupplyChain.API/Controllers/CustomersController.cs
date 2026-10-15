using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartSupplyChain.Application.Interfaces;
using SmartSupplyChain.Core.DTOs.Customers;

namespace SmartSupplyChain.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var customers = await _customerService.GetAllAsync();

        return Ok(customers);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var customer = await _customerService.GetByIdAsync(id);

        if (customer is null)
        {
            return NotFound(new
            {
                Message = "Customer not found."
            });
        }

        return Ok(customer);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateCustomerDto request)
    {
        var created = await _customerService.CreateAsync(request);

        if (!created)
        {
            return BadRequest(new
            {
                Message = "Unable to create customer."
            });
        }

        return Ok(new
        {
            Message = "Customer created successfully."
        });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateCustomerDto request)
    {
        var updated = await _customerService.UpdateAsync(id, request);

        if (!updated)
        {
            return NotFound(new
            {
                Message = "Customer not found."
            });
        }

        return Ok(new
        {
            Message = "Customer updated successfully."
        });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _customerService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(new
            {
                Message = "Customer not found."
            });
        }

        return Ok(new
        {
            Message = "Customer deleted successfully."
        });
    }
}