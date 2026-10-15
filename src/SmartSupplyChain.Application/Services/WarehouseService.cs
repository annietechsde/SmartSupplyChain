using SmartSupplyChain.Core.Entities;
using SmartSupplyChain.Core.Interfaces;

namespace SmartSupplyChain.Application.Services;

public class WarehouseService
{
    private readonly IUnitOfWork _unitOfWork;

    public WarehouseService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Warehouse>> GetAllAsync()
    {
        return await _unitOfWork.Warehouses.GetAllAsync();
    }

    public async Task<Warehouse?> GetByIdAsync(int id)
    {
        return await _unitOfWork.Warehouses.GetByIdAsync(id);
    }

    public async Task<bool> CreateAsync(Warehouse warehouse)
    {
        await _unitOfWork.Warehouses.AddAsync(warehouse);

        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UpdateAsync(
        int id,
        Warehouse warehouse)
    {
        var existingWarehouse =
            await _unitOfWork.Warehouses.GetByIdAsync(id);

        if (existingWarehouse is null)
        {
            return false;
        }

        existingWarehouse.Name = warehouse.Name;
        existingWarehouse.Code = warehouse.Code;
        existingWarehouse.AddressId = warehouse.AddressId;
        existingWarehouse.ContactNumber = warehouse.ContactNumber;
        existingWarehouse.IsActive = warehouse.IsActive;

        _unitOfWork.Warehouses.Update(existingWarehouse);

        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var warehouse =
            await _unitOfWork.Warehouses.GetByIdAsync(id);

        if (warehouse is null)
        {
            return false;
        }

        _unitOfWork.Warehouses.Delete(warehouse);

        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}