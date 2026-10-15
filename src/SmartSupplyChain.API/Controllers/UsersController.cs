using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartSupplyChain.Application.Interfaces;
using SmartSupplyChain.Core.DTOs.Users;

namespace SmartSupplyChain.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAllAsync();

        return Ok(users);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _userService.GetByIdAsync(id);

        if (user is null)
        {
            return NotFound(new
            {
                Message = "User not found."
            });
        }

        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateUserDto request)
    {
        var created = await _userService.CreateAsync(request);

        if (!created)
        {
            return BadRequest(new
            {
                Message = "Unable to create user."
            });
        }

        return Ok(new
        {
            Message = "User created successfully."
        });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateUserDto request)
    {
        var updated = await _userService.UpdateAsync(id, request);

        if (!updated)
        {
            return NotFound(new
            {
                Message = "User not found."
            });
        }

        return Ok(new
        {
            Message = "User updated successfully."
        });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _userService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(new
            {
                Message = "User not found."
            });
        }

        return Ok(new
        {
            Message = "User deleted successfully."
        });
    }
}