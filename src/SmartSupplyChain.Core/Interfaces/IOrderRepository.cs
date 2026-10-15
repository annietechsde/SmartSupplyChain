
using SmartSupplyChain.Core.Entities;

namespace SmartSupplyChain.Core.Interfaces;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(int id);

    Task<IEnumerable<Order>> GetAllAsync();

    Task<IEnumerable<Order>> GetByCustomerIdAsync(int customerId);

    Task<Order?> GetByExternalOrderIdAsync(string externalOrderId);

    Task AddAsync(Order order);

    void Update(Order order);

    void Delete(Order order);
}
