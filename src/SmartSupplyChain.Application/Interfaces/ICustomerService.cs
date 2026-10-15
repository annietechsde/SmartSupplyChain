using SmartSupplyChain.Core.DTOs.Customers;

namespace SmartSupplyChain.Application.Interfaces;

public interface ICustomerService
{
    Task<IEnumerable<CustomerDto>> GetAllAsync();

    Task<CustomerDto?> GetByIdAsync(int id);

    Task<bool> CreateAsync(CreateCustomerDto request);

    Task<bool> UpdateAsync(int id, UpdateCustomerDto request);

    Task<bool> DeleteAsync(int id);
}