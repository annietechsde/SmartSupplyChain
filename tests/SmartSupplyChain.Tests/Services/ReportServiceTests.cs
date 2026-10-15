using Moq;
using SmartSupplyChain.Application.Services;
using SmartSupplyChain.Core.Entities;
using SmartSupplyChain.Core.Enums;
using SmartSupplyChain.Core.Interfaces;

namespace SmartSupplyChain.Tests.Services;

public class ReportServiceTests
{
    [Fact]
    public async Task GetOrderReportAsync_ReturnsCorrectOrderCounts()
    {
        var unitOfWork = new Mock<IUnitOfWork>();

        var orders = new List<Order>
        {
            new()
            {
                Id = 1,
                Status = OrderStatus.Pending,
                TotalAmount = 1000
            },
            new()
            {
                Id = 2,
                Status = OrderStatus.Confirmed,
                TotalAmount = 2000
            },
            new()
            {
                Id = 3,
                Status = OrderStatus.Delivered,
                TotalAmount = 3000
            }
        };

        unitOfWork
            .Setup(unit => unit.Orders.GetAllAsync())
            .ReturnsAsync(orders);

        var service = new ReportService(
            unitOfWork.Object);

        var result = await service.GetOrderReportAsync();

        var resultType = result.GetType();

        Assert.Equal(
            3,
            resultType.GetProperty("TotalOrders")!.GetValue(result));

        Assert.Equal(
            1,
            resultType.GetProperty("PendingOrders")!.GetValue(result));

        Assert.Equal(
            1,
            resultType.GetProperty("ConfirmedOrders")!.GetValue(result));

        Assert.Equal(
            1,
            resultType.GetProperty("DeliveredOrders")!.GetValue(result));

        Assert.Equal(
            6000m,
            resultType.GetProperty("TotalOrderValue")!.GetValue(result));
    }

    [Fact]
    public async Task GetShipmentReportAsync_ReturnsCorrectShipmentCounts()
    {
        var unitOfWork = new Mock<IUnitOfWork>();

        var shipments = new List<Shipment>
        {
            new()
            {
                Id = 1,
                Status = ShipmentStatus.Pending
            },
            new()
            {
                Id = 2,
                Status = ShipmentStatus.InTransit
            },
            new()
            {
                Id = 3,
                Status = ShipmentStatus.Delivered
            }
        };

        unitOfWork
            .Setup(unit => unit.Shipments.GetAllAsync())
            .ReturnsAsync(shipments);

        var service = new ReportService(
            unitOfWork.Object);

        var result = await service.GetShipmentReportAsync();

        var resultType = result.GetType();

        Assert.Equal(
            3,
            resultType.GetProperty("TotalShipments")!.GetValue(result));

        Assert.Equal(
            1,
            resultType.GetProperty("PendingShipments")!.GetValue(result));

        Assert.Equal(
            1,
            resultType.GetProperty("InTransitShipments")!.GetValue(result));

        Assert.Equal(
            1,
            resultType.GetProperty("DeliveredShipments")!.GetValue(result));
    }

    [Fact]
    public async Task GetPaymentReportAsync_ReturnsOnlyPaymentData()
    {
        var unitOfWork = new Mock<IUnitOfWork>();

        var orders = new List<Order>
        {
            new()
            {
                Id = 1,
                Payment = new Payment
                {
                    Amount = 1000,
                    Status = PaymentStatus.Completed
                }
            },
            new()
            {
                Id = 2,
                Payment = new Payment
                {
                    Amount = 500,
                    Status = PaymentStatus.Pending
                }
            },
            new()
            {
                Id = 3,
                Payment = null
            }
        };

        unitOfWork
            .Setup(unit => unit.Orders.GetAllAsync())
            .ReturnsAsync(orders);

        var service = new ReportService(
            unitOfWork.Object);

        var result = await service.GetPaymentReportAsync();

        var resultType = result.GetType();

        Assert.Equal(
            2,
            resultType.GetProperty("TotalPayments")!.GetValue(result));

        Assert.Equal(
            1,
            resultType.GetProperty("CompletedPayments")!.GetValue(result));

        Assert.Equal(
            1,
            resultType.GetProperty("PendingPayments")!.GetValue(result));

        Assert.Equal(
            1000m,
            resultType.GetProperty("TotalPaidAmount")!.GetValue(result));
    }

    [Fact]
    public async Task GetDeliveryReportAsync_ReturnsCorrectDeliveryCounts()
    {
        var unitOfWork = new Mock<IUnitOfWork>();

        var shipments = new List<Shipment>
        {
            new()
            {
                Id = 1,
                Status = ShipmentStatus.Delivered
            },
            new()
            {
                Id = 2,
                Status = ShipmentStatus.OutForDelivery
            },
            new()
            {
                Id = 3,
                Status = ShipmentStatus.InTransit
            },
            new()
            {
                Id = 4,
                Status = ShipmentStatus.Returned
            }
        };

        unitOfWork
            .Setup(unit => unit.Shipments.GetAllAsync())
            .ReturnsAsync(shipments);

        var service = new ReportService(
            unitOfWork.Object);

        var result = await service.GetDeliveryReportAsync();

        var resultType = result.GetType();

        Assert.Equal(
            4,
            resultType.GetProperty("TotalDeliveries")!.GetValue(result));

        Assert.Equal(
            1,
            resultType.GetProperty("Delivered")!.GetValue(result));

        Assert.Equal(
            1,
            resultType.GetProperty("OutForDelivery")!.GetValue(result));

        Assert.Equal(
            1,
            resultType.GetProperty("InTransit")!.GetValue(result));

        Assert.Equal(
            1,
            resultType.GetProperty("Returned")!.GetValue(result));
    }
}