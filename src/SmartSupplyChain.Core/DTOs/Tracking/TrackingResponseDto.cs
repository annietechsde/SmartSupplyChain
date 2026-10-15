using SmartSupplyChain.Core.Enums;

namespace SmartSupplyChain.Core.DTOs.Tracking;

public class TrackingResponseDto
{
    public string TrackingNumber { get; set; } = string.Empty;

    public ShipmentStatus CurrentStatus { get; set; }

    public string CurrentLocation { get; set; } = string.Empty;

    public DateTime? EstimatedDeliveryDate { get; set; }

    public List<TrackingHistoryDto> History { get; set; } = new();
}

public class TrackingHistoryDto
{
    public ShipmentStatus Status { get; set; }

    public string Location { get; set; } = string.Empty;

    public string? Remarks { get; set; }

    public DateTime UpdatedAt { get; set; }
}