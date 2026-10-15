
using SmartSupplyChain.Core.DTOs.Orders;

namespace SmartSupplyChain.Application.Interfaces;

public interface IOrderService
{
    Task<IEnumerable<OrderDto>> GetAllAsync(int? userId = null);

    Task<OrderDto?> GetByIdAsync(
        int id,
        int? userId = null);

    Task<bool> CreateAsync(
        CreateOrderDto request,
        int? userId = null);

    Task<bool> CreateExternalOrderAsync(
        ExternalOrderDto request);

    Task<bool> DeleteAsync(
        int id,
        int? userId = null);
}
