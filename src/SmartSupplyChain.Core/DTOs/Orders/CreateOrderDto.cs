using System.Collections.Generic;

namespace SmartSupplyChain.Core.DTOs.Orders;

public class CreateOrderDto
{
    public int CustomerId { get; set; }

    public DateTime? ExpectedDeliveryDate { get; set; }

    public List<CreateOrderItemDto> Items { get; set; } = new();
}

public class CreateOrderItemDto
{
    public string ProductName { get; set; } = string.Empty;

    public string? ProductCode { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }
}