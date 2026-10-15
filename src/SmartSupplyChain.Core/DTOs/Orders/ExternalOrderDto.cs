
namespace SmartSupplyChain.Core.DTOs.Orders;

public class ExternalOrderDto
{
    public string ExternalOrderId { get; set; } = string.Empty;

    public string CustomerName { get; set; } = string.Empty;

    public string CustomerEmail { get; set; } = string.Empty;

    public string CustomerPhone { get; set; } = string.Empty;

    public string AddressLine1 { get; set; } = string.Empty;

    public string? AddressLine2 { get; set; }

    public string City { get; set; } = string.Empty;

    public string State { get; set; } = string.Empty;

    public string PostalCode { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;

    public DateTime? ExpectedDeliveryDate { get; set; }

    public List<ExternalOrderItemDto> Items { get; set; } = new();
}

public class ExternalOrderItemDto
{
    public string ProductName { get; set; } = string.Empty;

    public string? ProductCode { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }
}
