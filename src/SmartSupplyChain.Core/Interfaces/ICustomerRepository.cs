using SmartSupplyChain.Core.Entities;

namespace SmartSupplyChain.Core.Interfaces;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(int id);

    Task<Customer?> GetByUserIdAsync(int userId);

    Task<Customer?> GetByEmailAsync(string email);

    Task<IEnumerable<Customer>> GetAllAsync();

    Task AddAsync(Customer customer);

    void Update(Customer customer);

    void Delete(Customer customer);
}