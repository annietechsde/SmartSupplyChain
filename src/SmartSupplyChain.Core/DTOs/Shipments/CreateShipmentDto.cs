namespace SmartSupplyChain.Core.DTOs.Shipments;

public class CreateShipmentDto
{
    public int OrderId { get; set; }

    public int? WarehouseId { get; set; }

    public DateTime? EstimatedDeliveryDate { get; set; }
}