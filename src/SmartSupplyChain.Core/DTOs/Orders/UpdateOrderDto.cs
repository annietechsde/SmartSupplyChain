using SmartSupplyChain.Core.Enums;

namespace SmartSupplyChain.Core.DTOs.Orders;

public class UpdateOrderDto
{
    public OrderStatus Status { get; set; }

    public DateTime? ExpectedDeliveryDate { get; set; }
}