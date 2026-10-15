namespace SmartSupplyChain.Application.Interfaces;

public interface IDeliveryService
{
    Task<bool> AssignShipmentAsync(int shipmentId, int deliveryAgentId);

    Task<bool> AcceptAssignmentAsync(int assignmentId);

    Task<bool> CompleteDeliveryAsync(int assignmentId);

    Task<bool> UpdateDeliveryStatusAsync(int assignmentId, string status);

    Task<IEnumerable<object>> GetAgentAssignmentsAsync(int deliveryAgentId);
}