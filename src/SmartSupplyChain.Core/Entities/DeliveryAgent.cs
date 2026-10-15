namespace SmartSupplyChain.Core.Entities;

public class DeliveryAgent
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public User? User { get; set; }

    public string EmployeeCode { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string VehicleNumber { get; set; } = string.Empty;

    public bool IsAvailable { get; set; } = true;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<DeliveryAssignment> Assignments { get; set; } =
        new List<DeliveryAssignment>();
}