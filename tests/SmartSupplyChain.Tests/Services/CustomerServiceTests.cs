using Moq;
using SmartSupplyChain.Application.Services;
using SmartSupplyChain.Application.Validators;
using SmartSupplyChain.Core.DTOs.Customers;
using SmartSupplyChain.Core.Entities;
using SmartSupplyChain.Core.Interfaces;

namespace SmartSupplyChain.Tests.Services;

public class CustomerServiceTests
{
    [Fact]
    public async Task CreateAsync_ReturnsFalse_WhenCustomerDataIsInvalid()
    {
        var unitOfWork = new Mock<IUnitOfWork>();
        var validator = new CustomerValidator();

        var service = new CustomerService(
            unitOfWork.Object,
            validator);

        var request = new CreateCustomerDto
        {
            Name = "",
            Email = "",
            PhoneNumber = ""
        };

        var result = await service.CreateAsync(request);

        Assert.False(result);

        unitOfWork.Verify(
            unit => unit.Customers.AddAsync(
                It.IsAny<Customer>()),
            Times.Never);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenCustomerDoesNotExist()
    {
        var unitOfWork = new Mock<IUnitOfWork>();
        var validator = new CustomerValidator();

        unitOfWork
            .Setup(unit => unit.Customers.GetByIdAsync(
                It.IsAny<int>()))
            .ReturnsAsync((Customer?)null);

        var service = new CustomerService(
            unitOfWork.Object,
            validator);

        var result = await service.GetByIdAsync(999);

        Assert.Null(result);
    }
}