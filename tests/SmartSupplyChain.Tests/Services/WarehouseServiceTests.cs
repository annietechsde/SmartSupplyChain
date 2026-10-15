using Moq;
using SmartSupplyChain.Application.Services;
using SmartSupplyChain.Core.Entities;
using SmartSupplyChain.Core.Interfaces;

namespace SmartSupplyChain.Tests.Services;

public class WarehouseServiceTests
{
    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenWarehouseDoesNotExist()
    {
        var unitOfWork = new Mock<IUnitOfWork>();

        unitOfWork
            .Setup(unit => unit.Warehouses.GetByIdAsync(1))
            .ReturnsAsync((Warehouse?)null);

        var service = new WarehouseService(
            unitOfWork.Object);

        var result = await service.GetByIdAsync(1);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsWarehouse_WhenWarehouseExists()
    {
        var unitOfWork = new Mock<IUnitOfWork>();

        var warehouse = new Warehouse
        {
            Id = 1,
            Name = "Main Warehouse",
            Code = "WH-001",
            AddressId = 1,
            ContactNumber = "9876543210",
            IsActive = true
        };

        unitOfWork
            .Setup(unit => unit.Warehouses.GetByIdAsync(1))
            .ReturnsAsync(warehouse);

        var service = new WarehouseService(
            unitOfWork.Object);

        var result = await service.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("Main Warehouse", result!.Name);
        Assert.Equal("WH-001", result.Code);
    }

    [Fact]
    public async Task CreateAsync_AddsWarehouse_AndSavesChanges()
    {
        var unitOfWork = new Mock<IUnitOfWork>();
        var warehouseRepository = new Mock<IWarehouseRepository>();

        unitOfWork
            .SetupGet(unit => unit.Warehouses)
            .Returns(warehouseRepository.Object);

        var service = new WarehouseService(
            unitOfWork.Object);

        var warehouse = new Warehouse
        {
            Name = "Main Warehouse",
            Code = "WH-001",
            AddressId = 1,
            ContactNumber = "9876543210",
            IsActive = true
        };

        var result = await service.CreateAsync(warehouse);

        Assert.True(result);

        warehouseRepository.Verify(
            repository => repository.AddAsync(warehouse),
            Times.Once);

        unitOfWork.Verify(
            unit => unit.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsFalse_WhenWarehouseDoesNotExist()
    {
        var unitOfWork = new Mock<IUnitOfWork>();

        unitOfWork
            .Setup(unit => unit.Warehouses.GetByIdAsync(1))
            .ReturnsAsync((Warehouse?)null);

        var service = new WarehouseService(
            unitOfWork.Object);

        var warehouse = new Warehouse
        {
            Name = "Updated Warehouse",
            Code = "WH-002",
            AddressId = 2,
            ContactNumber = "9999999999",
            IsActive = false
        };

        var result = await service.UpdateAsync(
            1,
            warehouse);

        Assert.False(result);

        unitOfWork.Verify(
            unit => unit.Warehouses.Update(
                It.IsAny<Warehouse>()),
            Times.Never);

        unitOfWork.Verify(
            unit => unit.SaveChangesAsync(),
            Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesWarehouse_AndSavesChanges()
    {
        var unitOfWork = new Mock<IUnitOfWork>();

        var existingWarehouse = new Warehouse
        {
            Id = 1,
            Name = "Old Warehouse",
            Code = "WH-001",
            AddressId = 1,
            ContactNumber = "9876543210",
            IsActive = true
        };

        unitOfWork
            .Setup(unit => unit.Warehouses.GetByIdAsync(1))
            .ReturnsAsync(existingWarehouse);

        var service = new WarehouseService(
            unitOfWork.Object);

        var updatedWarehouse = new Warehouse
        {
            Name = "Updated Warehouse",
            Code = "WH-002",
            AddressId = 2,
            ContactNumber = "9999999999",
            IsActive = false
        };

        var result = await service.UpdateAsync(
            1,
            updatedWarehouse);

        Assert.True(result);
        Assert.Equal(
            "Updated Warehouse",
            existingWarehouse.Name);
        Assert.Equal(
            "WH-002",
            existingWarehouse.Code);
        Assert.Equal(
            2,
            existingWarehouse.AddressId);
        Assert.Equal(
            "9999999999",
            existingWarehouse.ContactNumber);
        Assert.False(existingWarehouse.IsActive);

        unitOfWork.Verify(
            unit => unit.Warehouses.Update(
                existingWarehouse),
            Times.Once);

        unitOfWork.Verify(
            unit => unit.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalse_WhenWarehouseDoesNotExist()
    {
        var unitOfWork = new Mock<IUnitOfWork>();

        unitOfWork
            .Setup(unit => unit.Warehouses.GetByIdAsync(1))
            .ReturnsAsync((Warehouse?)null);

        var service = new WarehouseService(
            unitOfWork.Object);

        var result = await service.DeleteAsync(1);

        Assert.False(result);

        unitOfWork.Verify(
            unit => unit.Warehouses.Delete(
                It.IsAny<Warehouse>()),
            Times.Never);

        unitOfWork.Verify(
            unit => unit.SaveChangesAsync(),
            Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_DeletesWarehouse_AndSavesChanges()
    {
        var unitOfWork = new Mock<IUnitOfWork>();

        var warehouse = new Warehouse
        {
            Id = 1,
            Name = "Main Warehouse",
            Code = "WH-001",
            AddressId = 1
        };

        unitOfWork
            .Setup(unit => unit.Warehouses.GetByIdAsync(1))
            .ReturnsAsync(warehouse);

        var service = new WarehouseService(
            unitOfWork.Object);

        var result = await service.DeleteAsync(1);

        Assert.True(result);

        unitOfWork.Verify(
            unit => unit.Warehouses.Delete(warehouse),
            Times.Once);

        unitOfWork.Verify(
            unit => unit.SaveChangesAsync(),
            Times.Once);
    }
}