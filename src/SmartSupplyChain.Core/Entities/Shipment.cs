using SmartSupplyChain.Core.Enums;

namespace SmartSupplyChain.Core.Entities;

public class Shipment
{
    public int Id { get; set; }

    public string TrackingNumber { get; set; } = string.Empty;

    public int OrderId { get; set; }

    public Order? Order { get; set; }

    public ShipmentStatus Status { get; set; } = ShipmentStatus.Pending;

    public int? WarehouseId { get; set; }

    public Warehouse? Warehouse { get; set; }

    public DateTime? ShippedAt { get; set; }

    public DateTime? EstimatedDeliveryDate { get; set; }

    public DateTime? DeliveredAt { get; set; }

    public ICollection<ShipmentTracking> TrackingHistory { get; set; } = new List<ShipmentTracking>();
}