using SmartSupplyChain.Core.DTOs.Shipments;

namespace SmartSupplyChain.Application.Interfaces;

public interface IShipmentService
{
    Task<IEnumerable<CreateShipmentDto>> GetAllAsync();

    Task<CreateShipmentDto?> GetByIdAsync(int id);

    Task<CreateShipmentDto?> GetByTrackingNumberAsync(string trackingNumber);

    Task<IEnumerable<CreateShipmentDto>> GetByOrderIdAsync(int orderId);

    Task<bool> CreateAsync(CreateShipmentDto request);

    Task<bool> UpdateStatusAsync(int id, UpdateShipmentStatusDto request);

    Task<bool> DeleteAsync(int id);
}