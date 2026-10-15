namespace SmartSupplyChain.Core.Entities;

public class DeliveryAssignment
{
    public int Id { get; set; }

    public int ShipmentId { get; set; }

    public Shipment? Shipment { get; set; }

    public int DeliveryAgentId { get; set; }

    public DeliveryAgent? DeliveryAgent { get; set; }

    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

    public DateTime? AcceptedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public bool IsCompleted { get; set; }
}