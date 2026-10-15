using Microsoft.AspNetCore.Mvc;
using SmartSupplyChain.Application.Interfaces;
using SmartSupplyChain.Core.DTOs.Auth;

namespace SmartSupplyChain.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequestDto request)
    {
        var result = await _authService.LoginAsync(request);

        if (result is null)
        {
            return Unauthorized(new
            {
                Message = "Invalid email or password."
            });
        }

        return Ok(result);
    }

    [HttpGet("check-email")]
    public async Task<IActionResult> CheckEmail(
        [FromQuery] string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return BadRequest(new
            {
                Message = "Email is required."
            });
        }

        var exists = await _authService.UserExistsAsync(email);

        return Ok(new
        {
            Exists = exists
        });
    }
}