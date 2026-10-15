using Moq;
using SmartSupplyChain.Application.Services;
using SmartSupplyChain.Core.Entities;
using SmartSupplyChain.Core.Enums;
using SmartSupplyChain.Core.Interfaces;

namespace SmartSupplyChain.Tests.Services;

public class PaymentServiceTests
{
    [Fact]
    public async Task CreatePaymentAsync_ReturnsFalse_WhenOrderIdIsInvalid()
    {
        var unitOfWork = new Mock<IUnitOfWork>();

        var service = new PaymentService(
            unitOfWork.Object);

        var result = await service.CreatePaymentAsync(
            0,
            1000,
            PaymentMethod.UPI);

        Assert.False(result);

        unitOfWork.Verify(
            unit => unit.Orders.GetByIdAsync(
                It.IsAny<int>()),
            Times.Never);
    }

    [Fact]
    public async Task CreatePaymentAsync_ReturnsFalse_WhenAmountIsInvalid()
    {
        var unitOfWork = new Mock<IUnitOfWork>();

        var service = new PaymentService(
            unitOfWork.Object);

        var result = await service.CreatePaymentAsync(
            1,
            0,
            PaymentMethod.UPI);

        Assert.False(result);

        unitOfWork.Verify(
            unit => unit.Orders.GetByIdAsync(
                It.IsAny<int>()),
            Times.Never);
    }

    [Fact]
    public async Task CreatePaymentAsync_ReturnsFalse_WhenOrderDoesNotExist()
    {
        var unitOfWork = new Mock<IUnitOfWork>();

        unitOfWork
            .Setup(unit => unit.Orders.GetByIdAsync(1))
            .ReturnsAsync((Order?)null);

        var service = new PaymentService(
            unitOfWork.Object);

        var result = await service.CreatePaymentAsync(
            1,
            1000,
            PaymentMethod.UPI);

        Assert.False(result);
    }

    [Fact]
    public async Task CreatePaymentAsync_ReturnsFalse_WhenPaymentAlreadyExists()
    {
        var unitOfWork = new Mock<IUnitOfWork>();

        var order = new Order
        {
            Id = 1,
            Payment = new Payment
            {
                Id = 1,
                OrderId = 1,
                Amount = 1000,
                Method = PaymentMethod.UPI
            }
        };

        unitOfWork
            .Setup(unit => unit.Orders.GetByIdAsync(1))
            .ReturnsAsync(order);

        var service = new PaymentService(
            unitOfWork.Object);

        var result = await service.CreatePaymentAsync(
            1,
            2000,
            PaymentMethod.CreditCard);

        Assert.False(result);

        unitOfWork.Verify(
            unit => unit.SaveChangesAsync(),
            Times.Never);
    }

    [Fact]
    public async Task CreatePaymentAsync_CreatesPayment_WhenOrderIsValid()
    {
        var unitOfWork = new Mock<IUnitOfWork>();

        var order = new Order
        {
            Id = 1
        };

        unitOfWork
            .Setup(unit => unit.Orders.GetByIdAsync(1))
            .ReturnsAsync(order);

        var service = new PaymentService(
            unitOfWork.Object);

        var result = await service.CreatePaymentAsync(
            1,
            1500,
            PaymentMethod.UPI);

        Assert.True(result);
        Assert.NotNull(order.Payment);
        Assert.Equal(1500, order.Payment!.Amount);
        Assert.Equal(
            PaymentMethod.UPI,
            order.Payment.Method);
        Assert.Equal(
            PaymentStatus.Pending,
            order.Payment.Status);

        unitOfWork.Verify(
            unit => unit.Orders.Update(order),
            Times.Once);

        unitOfWork.Verify(
            unit => unit.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task UpdatePaymentStatusAsync_ReturnsFalse_WhenPaymentDoesNotExist()
    {
        var unitOfWork = new Mock<IUnitOfWork>();

        unitOfWork
            .Setup(unit => unit.Orders.GetByIdAsync(1))
            .ReturnsAsync((Order?)null);

        var service = new PaymentService(
            unitOfWork.Object);

        var result = await service.UpdatePaymentStatusAsync(
            1,
            PaymentStatus.Completed);

        Assert.False(result);

        unitOfWork.Verify(
            unit => unit.SaveChangesAsync(),
            Times.Never);
    }

    [Fact]
    public async Task UpdatePaymentStatusAsync_UpdatesStatus_WhenPaymentExists()
    {
        var unitOfWork = new Mock<IUnitOfWork>();

        var payment = new Payment
        {
            Id = 1,
            OrderId = 1,
            Amount = 1500,
            Method = PaymentMethod.UPI,
            Status = PaymentStatus.Pending
        };

        var order = new Order
        {
            Id = 1,
            Payment = payment
        };

        unitOfWork
            .Setup(unit => unit.Orders.GetByIdAsync(1))
            .ReturnsAsync(order);

        var service = new PaymentService(
            unitOfWork.Object);

        var result = await service.UpdatePaymentStatusAsync(
            1,
            PaymentStatus.Completed,
            "TXN-001");

        Assert.True(result);
        Assert.Equal(
            PaymentStatus.Completed,
            payment.Status);
        Assert.Equal(
            "TXN-001",
            payment.TransactionId);
        Assert.NotNull(payment.PaidAt);

        unitOfWork.Verify(
            unit => unit.Orders.Update(order),
            Times.Once);

        unitOfWork.Verify(
            unit => unit.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task GetPaymentByOrderIdAsync_ReturnsNull_WhenPaymentDoesNotExist()
    {
        var unitOfWork = new Mock<IUnitOfWork>();

        unitOfWork
            .Setup(unit => unit.Orders.GetByIdAsync(1))
            .ReturnsAsync((Order?)null);

        var service = new PaymentService(
            unitOfWork.Object);

        var result =
            await service.GetPaymentByOrderIdAsync(1);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetPaymentByOrderIdAsync_ReturnsPayment_WhenPaymentExists()
    {
        var unitOfWork = new Mock<IUnitOfWork>();

        var payment = new Payment
        {
            Id = 1,
            OrderId = 1,
            Amount = 1500,
            Method = PaymentMethod.UPI,
            Status = PaymentStatus.Completed,
            TransactionId = "TXN-001"
        };

        var order = new Order
        {
            Id = 1,
            Payment = payment
        };

        unitOfWork
            .Setup(unit => unit.Orders.GetByIdAsync(1))
            .ReturnsAsync(order);

        var service = new PaymentService(
            unitOfWork.Object);

        var result =
            await service.GetPaymentByOrderIdAsync(1);

        Assert.NotNull(result);
    }
}