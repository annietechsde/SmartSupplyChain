using Moq;
using SmartSupplyChain.Application.Services;
using SmartSupplyChain.Core.Entities;
using SmartSupplyChain.Core.Enums;
using SmartSupplyChain.Core.Interfaces;

namespace SmartSupplyChain.Tests.Services;

public class TrackingServiceTests
{
    [Fact]
    public async Task GetTrackingAsync_ReturnsNull_WhenTrackingNumberIsEmpty()
    {
        var unitOfWork = new Mock<IUnitOfWork>();

        var service = new TrackingService(
            unitOfWork.Object);

        var result = await service.GetTrackingAsync("");

        Assert.Null(result);

        unitOfWork.Verify(
            unit => unit.Shipments.GetByTrackingNumberAsync(
                It.IsAny<string>()),
            Times.Never);
    }

    [Fact]
    public async Task GetTrackingAsync_ReturnsNull_WhenShipmentDoesNotExist()
    {
        var unitOfWork = new Mock<IUnitOfWork>();

        unitOfWork
            .Setup(unit => unit.Shipments.GetByTrackingNumberAsync(
                "TRK-001"))
            .ReturnsAsync((Shipment?)null);

        var service = new TrackingService(
            unitOfWork.Object);

        var result = await service.GetTrackingAsync("TRK-001");

        Assert.Null(result);
    }

    [Fact]
    public async Task GetTrackingAsync_ReturnsTrackingInformation_WhenShipmentExists()
    {
        var unitOfWork = new Mock<IUnitOfWork>();

        var shipment = new Shipment
        {
            Id = 1,
            TrackingNumber = "TRK-001",
            Status = ShipmentStatus.InTransit,
            EstimatedDeliveryDate =
                DateTime.UtcNow.AddDays(3)
        };

        shipment.TrackingHistory.Add(
            new ShipmentTracking
            {
                Status = ShipmentStatus.Pending,
                Location = "Warehouse",
                Remarks = "Shipment created",
                UpdatedAt = DateTime.UtcNow.AddHours(-2)
            });

        shipment.TrackingHistory.Add(
            new ShipmentTracking
            {
                Status = ShipmentStatus.InTransit,
                Location = "Delhi",
                Remarks = "Shipment dispatched",
                UpdatedAt = DateTime.UtcNow
            });

        unitOfWork
            .Setup(unit => unit.Shipments.GetByTrackingNumberAsync(
                "TRK-001"))
            .ReturnsAsync(shipment);

        var service = new TrackingService(
            unitOfWork.Object);

        var result = await service.GetTrackingAsync("TRK-001");

        Assert.NotNull(result);
        Assert.Equal("TRK-001", result!.TrackingNumber);
        Assert.Equal(
            ShipmentStatus.InTransit,
            result.CurrentStatus);
        Assert.Equal("Delhi", result.CurrentLocation);
        Assert.Equal(2, result.History.Count);
    }

    [Fact]
    public async Task GetTrackingAsync_UsesLatestTrackingLocation()
    {
        var unitOfWork = new Mock<IUnitOfWork>();

        var shipment = new Shipment
        {
            Id = 1,
            TrackingNumber = "TRK-002",
            Status = ShipmentStatus.OutForDelivery
        };

        shipment.TrackingHistory.Add(
            new ShipmentTracking
            {
                Status = ShipmentStatus.Pending,
                Location = "Warehouse",
                UpdatedAt = DateTime.UtcNow.AddHours(-3)
            });

        shipment.TrackingHistory.Add(
            new ShipmentTracking
            {
                Status = ShipmentStatus.InTransit,
                Location = "Delhi",
                UpdatedAt = DateTime.UtcNow.AddHours(-2)
            });

        shipment.TrackingHistory.Add(
            new ShipmentTracking
            {
                Status = ShipmentStatus.OutForDelivery,
                Location = "Greater Noida",
                UpdatedAt = DateTime.UtcNow
            });

        unitOfWork
            .Setup(unit => unit.Shipments.GetByTrackingNumberAsync(
                "TRK-002"))
            .ReturnsAsync(shipment);

        var service = new TrackingService(
            unitOfWork.Object);

        var result = await service.GetTrackingAsync("TRK-002");

        Assert.NotNull(result);
        Assert.Equal(
            "Greater Noida",
            result!.CurrentLocation);
    }
}