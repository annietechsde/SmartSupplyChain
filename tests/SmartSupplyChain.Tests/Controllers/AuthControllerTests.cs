using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartSupplyChain.API.Controllers;
using SmartSupplyChain.Application.Interfaces;
using SmartSupplyChain.Core.DTOs.Auth;

namespace SmartSupplyChain.Tests.Controllers;

public class AuthControllerTests
{
    [Fact]
    public async Task Login_ReturnsUnauthorized_WhenCredentialsAreInvalid()
    {
        var authService = new Mock<IAuthService>();

        authService
            .Setup(service => service.LoginAsync(
                It.IsAny<LoginRequestDto>()))
            .ReturnsAsync((LoginResponseDto?)null);

        var controller = new AuthController(authService.Object);

        var request = new LoginRequestDto
        {
            Email = "wrong@example.com",
            Password = "WrongPassword"
        };

        var result = await controller.Login(request);

        var unauthorizedResult =
            Assert.IsType<UnauthorizedObjectResult>(result);

        Assert.Equal(401, unauthorizedResult.StatusCode);
    }

    [Fact]
    public async Task Login_ReturnsOk_WhenCredentialsAreValid()
    {
        var authService = new Mock<IAuthService>();

        var loginResponse = new LoginResponseDto
        {
            UserId = 1,
            FirstName = "Test",
            LastName = "User",
            Email = "test@example.com",
            Role = "Customer",
            Token = "sample-jwt-token"
        };

        authService
            .Setup(service => service.LoginAsync(
                It.IsAny<LoginRequestDto>()))
            .ReturnsAsync(loginResponse);

        var controller = new AuthController(authService.Object);

        var request = new LoginRequestDto
        {
            Email = "test@example.com",
            Password = "Password123"
        };

        var result = await controller.Login(request);

        var okResult =
            Assert.IsType<OkObjectResult>(result);

        Assert.Equal(loginResponse, okResult.Value);
    }
}