using Moq;
using SmartSupplyChain.Application.Interfaces;
using SmartSupplyChain.Application.Services;
using SmartSupplyChain.Core.DTOs.Shipments;
using SmartSupplyChain.Core.Entities;
using SmartSupplyChain.Core.Enums;
using SmartSupplyChain.Core.Interfaces;

namespace SmartSupplyChain.Tests.Services;

public class DeliveryServiceTests
{
    [Fact]
    public async Task AssignShipmentAsync_ReturnsFalse_WhenIdsAreInvalid()
    {
        var unitOfWork = new Mock<IUnitOfWork>();
        var shipmentService = new Mock<IShipmentService>();

        var service = new DeliveryService(
            unitOfWork.Object,
            shipmentService.Object);

        var result = await service.AssignShipmentAsync(0, 1);

        Assert.False(result);

        unitOfWork.Verify(
            unit => unit.Delivery.AddAsync(
                It.IsAny<DeliveryAssignment>()),
            Times.Never);
    }

    [Fact]
    public async Task AcceptAssignmentAsync_ReturnsFalse_WhenAssignmentDoesNotExist()
    {
        var unitOfWork = new Mock<IUnitOfWork>();
        var shipmentService = new Mock<IShipmentService>();

        unitOfWork
            .Setup(unit => unit.Delivery.GetByIdAsync(
                It.IsAny<int>()))
            .ReturnsAsync((DeliveryAssignment?)null);

        var service = new DeliveryService(
            unitOfWork.Object,
            shipmentService.Object);

        var result = await service.AcceptAssignmentAsync(1);

        Assert.False(result);

        unitOfWork.Verify(
            unit => unit.SaveChangesAsync(),
            Times.Never);
    }

    [Fact]
    public async Task AcceptAssignmentAsync_SavesChanges_WhenAssignmentExists()
    {
        var unitOfWork = new Mock<IUnitOfWork>();
        var shipmentService = new Mock<IShipmentService>();

        var assignment = new DeliveryAssignment
        {
            Id = 1,
            ShipmentId = 1,
            DeliveryAgentId = 1
        };

        unitOfWork
            .Setup(unit => unit.Delivery.GetByIdAsync(1))
            .ReturnsAsync(assignment);

        var service = new DeliveryService(
            unitOfWork.Object,
            shipmentService.Object);

        var result = await service.AcceptAssignmentAsync(1);

        Assert.True(result);
        Assert.NotNull(assignment.AcceptedAt);

        unitOfWork.Verify(
            unit => unit.Delivery.Update(assignment),
            Times.Once);

        unitOfWork.Verify(
            unit => unit.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task CompleteDeliveryAsync_ReturnsFalse_WhenAssignmentDoesNotExist()
    {
        var unitOfWork = new Mock<IUnitOfWork>();
        var shipmentService = new Mock<IShipmentService>();

        unitOfWork
            .Setup(unit => unit.Delivery.GetByIdAsync(
                It.IsAny<int>()))
            .ReturnsAsync((DeliveryAssignment?)null);

        var service = new DeliveryService(
            unitOfWork.Object,
            shipmentService.Object);

        var result = await service.CompleteDeliveryAsync(1);

        Assert.False(result);

        unitOfWork.Verify(
            unit => unit.SaveChangesAsync(),
            Times.Never);

        shipmentService.Verify(
            service => service.UpdateStatusAsync(
                It.IsAny<int>(),
                It.IsAny<UpdateShipmentStatusDto>()),
            Times.Never);
    }

    [Fact]
    public async Task CompleteDeliveryAsync_SavesChanges_WhenAssignmentExists()
    {
        var unitOfWork = new Mock<IUnitOfWork>();
        var shipmentService = new Mock<IShipmentService>();

        var assignment = new DeliveryAssignment
        {
            Id = 1,
            ShipmentId = 1,
            DeliveryAgentId = 1
        };

        unitOfWork
            .Setup(unit => unit.Delivery.GetByIdAsync(1))
            .ReturnsAsync(assignment);

        shipmentService
            .Setup(service => service.UpdateStatusAsync(
                1,
                It.IsAny<UpdateShipmentStatusDto>()))
            .ReturnsAsync(true);

        var service = new DeliveryService(
            unitOfWork.Object,
            shipmentService.Object);

        var result = await service.CompleteDeliveryAsync(1);

        Assert.True(result);
        Assert.True(assignment.IsCompleted);
        Assert.NotNull(assignment.CompletedAt);

        unitOfWork.Verify(
            unit => unit.Delivery.Update(assignment),
            Times.Once);

        unitOfWork.Verify(
            unit => unit.SaveChangesAsync(),
            Times.Once);

        shipmentService.Verify(
            service => service.UpdateStatusAsync(
                1,
                It.Is<UpdateShipmentStatusDto>(request =>
                    request.Status == ShipmentStatus.Delivered &&
                    request.Location == "Delivered" &&
                    request.Remarks ==
                        "Delivery completed successfully")),
            Times.Once);
    }

    [Fact]
    public async Task UpdateDeliveryStatusAsync_ReturnsFalse_ForInvalidStatus()
    {
        var unitOfWork = new Mock<IUnitOfWork>();
        var shipmentService = new Mock<IShipmentService>();

        var assignment = new DeliveryAssignment
        {
            Id = 1,
            ShipmentId = 1,
            DeliveryAgentId = 1
        };

        unitOfWork
            .Setup(unit => unit.Delivery.GetByIdAsync(1))
            .ReturnsAsync(assignment);

        var service = new DeliveryService(
            unitOfWork.Object,
            shipmentService.Object);

        var result = await service.UpdateDeliveryStatusAsync(
            1,
            "Invalid");

        Assert.False(result);

        unitOfWork.Verify(
            unit => unit.SaveChangesAsync(),
            Times.Never);
    }

    [Fact]
    public async Task UpdateDeliveryStatusAsync_Accepted_SavesChanges()
    {
        var unitOfWork = new Mock<IUnitOfWork>();
        var shipmentService = new Mock<IShipmentService>();

        var assignment = new DeliveryAssignment
        {
            Id = 1,
            ShipmentId = 1,
            DeliveryAgentId = 1
        };

        unitOfWork
            .Setup(unit => unit.Delivery.GetByIdAsync(1))
            .ReturnsAsync(assignment);

        var service = new DeliveryService(
            unitOfWork.Object,
            shipmentService.Object);

        var result = await service.UpdateDeliveryStatusAsync(
            1,
            "Accepted");

        Assert.True(result);
        Assert.NotNull(assignment.AcceptedAt);

        unitOfWork.Verify(
            unit => unit.Delivery.Update(assignment),
            Times.Once);

        unitOfWork.Verify(
            unit => unit.SaveChangesAsync(),
            Times.Once);

        shipmentService.Verify(
            service => service.UpdateStatusAsync(
                It.IsAny<int>(),
                It.IsAny<UpdateShipmentStatusDto>()),
            Times.Never);
    }

    [Fact]
    public async Task UpdateDeliveryStatusAsync_Completed_SavesChanges()
    {
        var unitOfWork = new Mock<IUnitOfWork>();
        var shipmentService = new Mock<IShipmentService>();

        var assignment = new DeliveryAssignment
        {
            Id = 1,
            ShipmentId = 1,
            DeliveryAgentId = 1
        };

        unitOfWork
            .Setup(unit => unit.Delivery.GetByIdAsync(1))
            .ReturnsAsync(assignment);

        shipmentService
            .Setup(service => service.UpdateStatusAsync(
                1,
                It.IsAny<UpdateShipmentStatusDto>()))
            .ReturnsAsync(true);

        var service = new DeliveryService(
            unitOfWork.Object,
            shipmentService.Object);

        var result = await service.UpdateDeliveryStatusAsync(
            1,
            "Completed");

        Assert.True(result);
        Assert.True(assignment.IsCompleted);
        Assert.NotNull(assignment.CompletedAt);

        unitOfWork.Verify(
            unit => unit.Delivery.Update(assignment),
            Times.Once);

        unitOfWork.Verify(
            unit => unit.SaveChangesAsync(),
            Times.Once);

        shipmentService.Verify(
            service => service.UpdateStatusAsync(
                1,
                It.Is<UpdateShipmentStatusDto>(request =>
                    request.Status == ShipmentStatus.Delivered &&
                    request.Location == "Delivered" &&
                    request.Remarks ==
                        "Delivery completed successfully")),
            Times.Once);
    }
}