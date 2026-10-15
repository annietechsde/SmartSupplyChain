using SmartSupplyChain.Core.Enums;
using SmartSupplyChain.Core.Interfaces;

namespace SmartSupplyChain.Application.Services;

public class ReportService
{
    private readonly IUnitOfWork _unitOfWork;

    public ReportService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<object> GetOrderReportAsync()
    {
        var orders = await _unitOfWork.Orders.GetAllAsync();

        return new
        {
            TotalOrders = orders.Count(),
            PendingOrders = orders.Count(
                order => order.Status == OrderStatus.Pending),
            ConfirmedOrders = orders.Count(
                order => order.Status == OrderStatus.Confirmed),
            ProcessingOrders = orders.Count(
                order => order.Status == OrderStatus.Processing),
            ShippedOrders = orders.Count(
                order => order.Status == OrderStatus.Shipped),
            DeliveredOrders = orders.Count(
                order => order.Status == OrderStatus.Delivered),
            CancelledOrders = orders.Count(
                order => order.Status == OrderStatus.Cancelled),
            TotalOrderValue = orders.Sum(
                order => order.TotalAmount)
        };
    }

    public async Task<object> GetShipmentReportAsync()
    {
        var shipments = await _unitOfWork.Shipments.GetAllAsync();

        return new
        {
            TotalShipments = shipments.Count(),
            PendingShipments = shipments.Count(
                shipment => shipment.Status == ShipmentStatus.Pending),
            InTransitShipments = shipments.Count(
                shipment => shipment.Status == ShipmentStatus.InTransit),
            OutForDelivery = shipments.Count(
                shipment => shipment.Status == ShipmentStatus.OutForDelivery),
            DeliveredShipments = shipments.Count(
                shipment => shipment.Status == ShipmentStatus.Delivered),
            ReturnedShipments = shipments.Count(
                shipment => shipment.Status == ShipmentStatus.Returned),
            CancelledShipments = shipments.Count(
                shipment => shipment.Status == ShipmentStatus.Cancelled)
        };
    }

    public async Task<object> GetPaymentReportAsync()
    {
        var orders = await _unitOfWork.Orders.GetAllAsync();

        var payments = orders
            .Where(order => order.Payment is not null)
            .Select(order => order.Payment!)
            .ToList();

        return new
        {
            TotalPayments = payments.Count,
            PendingPayments = payments.Count(
                payment => payment.Status == PaymentStatus.Pending),
            ProcessingPayments = payments.Count(
                payment => payment.Status == PaymentStatus.Processing),
            CompletedPayments = payments.Count(
                payment => payment.Status == PaymentStatus.Completed),
            FailedPayments = payments.Count(
                payment => payment.Status == PaymentStatus.Failed),
            RefundedPayments = payments.Count(
                payment => payment.Status == PaymentStatus.Refunded),
            TotalPaidAmount = payments
                .Where(payment =>
                    payment.Status == PaymentStatus.Completed)
                .Sum(payment => payment.Amount)
        };
    }

    public async Task<object> GetDeliveryReportAsync()
    {
        var shipments = await _unitOfWork.Shipments.GetAllAsync();

        return new
        {
            TotalDeliveries = shipments.Count(),
            Delivered = shipments.Count(
                shipment => shipment.Status == ShipmentStatus.Delivered),
            OutForDelivery = shipments.Count(
                shipment => shipment.Status == ShipmentStatus.OutForDelivery),
            InTransit = shipments.Count(
                shipment => shipment.Status == ShipmentStatus.InTransit),
            Returned = shipments.Count(
                shipment => shipment.Status == ShipmentStatus.Returned)
        };
    }
}