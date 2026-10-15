using Moq;
using SmartSupplyChain.Application.Services;
using SmartSupplyChain.Core.DTOs.Orders;
using SmartSupplyChain.Core.Entities;
using SmartSupplyChain.Core.Interfaces;

namespace SmartSupplyChain.Tests.Services;

public class OrderServiceTests
{
    [Fact]
    public async Task CreateAsync_ReturnsFalse_WhenCustomerDoesNotExist()
    {
        var unitOfWork = new Mock<IUnitOfWork>();

        unitOfWork
            .Setup(unit => unit.Customers.GetByIdAsync(
                It.IsAny<int>()))
            .ReturnsAsync((Customer?)null);

        var service = new OrderService(
            unitOfWork.Object);

        var request = new CreateOrderDto
        {
            CustomerId = 1,
            Items = new List<CreateOrderItemDto>
            {
                new()
                {
                    ProductName = "Laptop",
                    Quantity = 1,
                    UnitPrice = 50000
                }
            }
        };

        var result = await service.CreateAsync(request);

        Assert.False(result);

        unitOfWork.Verify(
            unit => unit.Orders.AddAsync(
                It.IsAny<Order>()),
            Times.Never);
    }
}