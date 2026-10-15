using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartSupplyChain.API.Controllers;
using SmartSupplyChain.Application.Interfaces;
using SmartSupplyChain.Core.DTOs.Orders;

namespace SmartSupplyChain.Tests.Controllers;

public class OrderControllerTests
{
    [Fact]
    public async Task GetById_ReturnsNotFound_WhenOrderDoesNotExist()
    {
        var orderService = new Mock<IOrderService>();

        orderService
            .Setup(service => service.GetByIdAsync(
                It.IsAny<int>(),
                null))
            .ReturnsAsync((OrderDto?)null);

        var controller = new OrdersController(
            orderService.Object);

        controller.ControllerContext =
            CreateAdminControllerContext();

        var result = await controller.GetById(999);

        var notFoundResult =
            Assert.IsType<NotFoundObjectResult>(result);

        Assert.Equal(404, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenOrderExists()
    {
        var orderService = new Mock<IOrderService>();

        var order = new OrderDto
        {
            Id = 1,
            CustomerId = 1,
            ExpectedDeliveryDate =
                DateTime.UtcNow.AddDays(5),

            Items = new List<CreateOrderItemDto>
            {
                new()
                {
                    ProductName = "Laptop",
                    ProductCode = "LAP-001",
                    Quantity = 1,
                    UnitPrice = 50000
                }
            }
        };

        orderService
            .Setup(service => service.GetByIdAsync(
                It.IsAny<int>(),
                null))
            .ReturnsAsync(order);

        var controller = new OrdersController(
            orderService.Object);

        controller.ControllerContext =
            CreateAdminControllerContext();

        var result = await controller.GetById(1);

        var okResult =
            Assert.IsType<OkObjectResult>(result);

        Assert.Equal(order, okResult.Value);
    }

    private static ControllerContext CreateAdminControllerContext()
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, "1"),
            new(ClaimTypes.Role, "Admin")
        };

        var identity = new ClaimsIdentity(
            claims,
            "TestAuthentication");

        var principal = new ClaimsPrincipal(identity);

        return new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = principal
            }
        };
    }
}