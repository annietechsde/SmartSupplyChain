using SmartSupplyChain.Core.Entities;
using SmartSupplyChain.Core.Enums;
using SmartSupplyChain.Core.Interfaces;

namespace SmartSupplyChain.Application.Services;

public class PaymentService
{
    private readonly IUnitOfWork _unitOfWork;

    public PaymentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> CreatePaymentAsync(
        int orderId,
        decimal amount,
        PaymentMethod method)
    {
        if (orderId <= 0 || amount <= 0)
        {
            return false;
        }

        var order = await _unitOfWork.Orders.GetByIdAsync(orderId);

        if (order is null)
        {
            return false;
        }

        if (order.Payment is not null)
        {
            return false;
        }

        var payment = new Payment
        {
            OrderId = orderId,
            Amount = amount,
            Method = method,
            Status = PaymentStatus.Pending
        };

        order.Payment = payment;

        _unitOfWork.Orders.Update(order);

        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UpdatePaymentStatusAsync(
        int orderId,
        PaymentStatus status,
        string? transactionId = null)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(orderId);

        if (order?.Payment is null)
        {
            return false;
        }

        order.Payment.Status = status;
        order.Payment.TransactionId = transactionId;

        if (status == PaymentStatus.Completed)
        {
            order.Payment.PaidAt = DateTime.UtcNow;
        }

        _unitOfWork.Orders.Update(order);

        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<object?> GetPaymentByOrderIdAsync(int orderId)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(orderId);

        if (order?.Payment is null)
        {
            return null;
        }

        return new
        {
            order.Payment.Id,
            order.Payment.OrderId,
            order.Payment.Amount,
            Method = order.Payment.Method.ToString(),
            Status = order.Payment.Status.ToString(),
            order.Payment.TransactionId,
            order.Payment.PaidAt,
            order.Payment.CreatedAt
        };
    }
}