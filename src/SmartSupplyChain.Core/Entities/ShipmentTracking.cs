using SmartSupplyChain.Core.Enums;

namespace SmartSupplyChain.Core.Entities;

public class ShipmentTracking
{
    public int Id { get; set; }

    public int ShipmentId { get; set; }

    public Shipment? Shipment { get; set; }

    public ShipmentStatus Status { get; set; }

    public string Location { get; set; } = string.Empty;

    public string? Remarks { get; set; }

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}