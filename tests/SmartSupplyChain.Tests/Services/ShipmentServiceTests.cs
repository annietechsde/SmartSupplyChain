using Moq;
using SmartSupplyChain.Application.Services;
using SmartSupplyChain.Application.Validators;
using SmartSupplyChain.Core.DTOs.Shipments;
using SmartSupplyChain.Core.Entities;
using SmartSupplyChain.Core.Interfaces;

namespace SmartSupplyChain.Tests.Services;

public class ShipmentServiceTests
{
    [Fact]
    public async Task CreateAsync_ReturnsFalse_WhenShipmentDataIsInvalid()
    {
        var unitOfWork = new Mock<IUnitOfWork>();
        var validator = new ShipmentValidator();

        var service = new ShipmentService(
            unitOfWork.Object,
            validator);

        var request = new CreateShipmentDto
        {
            OrderId = 0
        };

        var result = await service.CreateAsync(request);

        Assert.False(result);

        unitOfWork.Verify(
            unit => unit.Shipments.AddAsync(
                It.IsAny<Shipment>()),
            Times.Never);
    }

    [Fact]
    public async Task CreateAsync_ReturnsFalse_WhenOrderDoesNotExist()
    {
        var unitOfWork = new Mock<IUnitOfWork>();
        var validator = new ShipmentValidator();

        unitOfWork
            .Setup(unit => unit.Orders.GetByIdAsync(
                It.IsAny<int>()))
            .ReturnsAsync((Order?)null);

        var service = new ShipmentService(
            unitOfWork.Object,
            validator);

        var request = new CreateShipmentDto
        {
            OrderId = 1,
            WarehouseId = 1
        };

        var result = await service.CreateAsync(request);

        Assert.False(result);
    }
}