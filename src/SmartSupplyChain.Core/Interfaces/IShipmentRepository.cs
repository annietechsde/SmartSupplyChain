using SmartSupplyChain.Core.Entities;

namespace SmartSupplyChain.Core.Interfaces;

public interface IShipmentRepository
{
    Task<Shipment?> GetByIdAsync(int id);

    Task<Shipment?> GetByTrackingNumberAsync(string trackingNumber);

    Task<IEnumerable<Shipment>> GetAllAsync();

    Task<IEnumerable<Shipment>> GetByOrderIdAsync(int orderId);

    Task AddAsync(Shipment shipment);

    void Update(Shipment shipment);

    void Delete(Shipment shipment);
}