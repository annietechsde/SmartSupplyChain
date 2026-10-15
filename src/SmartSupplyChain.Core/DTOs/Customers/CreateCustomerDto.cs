namespace SmartSupplyChain.Core.DTOs.Customers;

public class CreateCustomerDto
{
    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public int? UserId { get; set; }

    public int? AddressId { get; set; }
}