using SmartSupplyChain.Core.Enums;

namespace SmartSupplyChain.Core.DTOs.Shipments;

public class UpdateShipmentStatusDto
{
    public ShipmentStatus Status { get; set; }

    public string Location { get; set; } = string.Empty;

    public string? Remarks { get; set; }
}