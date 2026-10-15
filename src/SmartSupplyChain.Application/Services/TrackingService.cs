using SmartSupplyChain.Core.DTOs.Tracking;
using SmartSupplyChain.Core.Interfaces;

namespace SmartSupplyChain.Application.Services;

public class TrackingService
{
    private readonly IUnitOfWork _unitOfWork;

    public TrackingService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TrackingResponseDto?> GetTrackingAsync(
        string trackingNumber)
    {
        if (string.IsNullOrWhiteSpace(trackingNumber))
        {
            return null;
        }

        var shipment = await _unitOfWork.Shipments
            .GetByTrackingNumberAsync(trackingNumber);

        if (shipment is null)
        {
            return null;
        }

        var latestTracking = shipment.TrackingHistory
            .OrderByDescending(tracking => tracking.UpdatedAt)
            .FirstOrDefault();

        return new TrackingResponseDto
        {
            TrackingNumber = shipment.TrackingNumber,
            CurrentStatus = shipment.Status,
            CurrentLocation = latestTracking?.Location ?? string.Empty,
            EstimatedDeliveryDate =
                shipment.EstimatedDeliveryDate,
            History = shipment.TrackingHistory
                .OrderByDescending(tracking => tracking.UpdatedAt)
                .Select(tracking => new TrackingHistoryDto
                {
                    Status = tracking.Status,
                    Location = tracking.Location,
                    Remarks = tracking.Remarks,
                    UpdatedAt = tracking.UpdatedAt
                })
                .ToList()
        };
    }
}