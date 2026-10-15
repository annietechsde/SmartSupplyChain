using SmartSupplyChain.Core.Entities;

namespace SmartSupplyChain.Core.Interfaces;

public interface IWarehouseRepository
{
    Task<Warehouse?> GetByIdAsync(int id);

    Task<IEnumerable<Warehouse>> GetAllAsync();

    Task AddAsync(Warehouse warehouse);

    void Update(Warehouse warehouse);

    void Delete(Warehouse warehouse);
}