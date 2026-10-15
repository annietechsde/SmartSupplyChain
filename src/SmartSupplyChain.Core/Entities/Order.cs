
using SmartSupplyChain.Core.Enums;

namespace SmartSupplyChain.Core.Entities;

public class Order
{
    public int Id { get; set; }

    public string OrderNumber { get; set; } = string.Empty;

    public string? ExternalOrderId { get; set; }

    public int CustomerId { get; set; }

    public Customer? Customer { get; set; }

    public int? DeliveryAddressId { get; set; }

    public Address? DeliveryAddress { get; set; }

    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    public decimal TotalAmount { get; set; }

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    public DateTime? ExpectedDeliveryDate { get; set; }

    public DateTime? DeliveredAt { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public Shipment? Shipment { get; set; }

    public Payment? Payment { get; set; }
}
