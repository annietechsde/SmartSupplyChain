namespace SmartSupplyChain.Core.DTOs.Orders;

public class OrderDto
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public DateTime? ExpectedDeliveryDate { get; set; }

    public List<CreateOrderItemDto> Items { get; set; } = new();
}