using SmartSupplyChain.Core.Entities;

namespace SmartSupplyChain.Core.Interfaces;

public interface IDeliveryRepository
{
    Task<DeliveryAssignment?> GetByIdAsync(int id);

    Task<IEnumerable<DeliveryAssignment>> GetByAgentIdAsync(
        int deliveryAgentId);

    Task AddAsync(DeliveryAssignment assignment);

    void Update(DeliveryAssignment assignment);

    void Delete(DeliveryAssignment assignment);
}