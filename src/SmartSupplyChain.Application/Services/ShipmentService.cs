using SmartSupplyChain.Application.Interfaces;
using SmartSupplyChain.Application.Validators;
using SmartSupplyChain.Core.DTOs.Shipments;
using SmartSupplyChain.Core.Entities;
using SmartSupplyChain.Core.Enums;
using SmartSupplyChain.Core.Interfaces;

namespace SmartSupplyChain.Application.Services;

public class ShipmentService : IShipmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ShipmentValidator _shipmentValidator;

    public ShipmentService(
        IUnitOfWork unitOfWork,
        ShipmentValidator shipmentValidator)
    {
        _unitOfWork = unitOfWork;
        _shipmentValidator = shipmentValidator;
    }

    public async Task<IEnumerable<CreateShipmentDto>> GetAllAsync()
    {
        var shipments = await _unitOfWork.Shipments.GetAllAsync();

        return shipments.Select(MapToDto);
    }

    public async Task<CreateShipmentDto?> GetByIdAsync(int id)
    {
        var shipment = await _unitOfWork.Shipments.GetByIdAsync(id);

        if (shipment is null)
        {
            return null;
        }

        return MapToDto(shipment);
    }

    public async Task<CreateShipmentDto?> GetByTrackingNumberAsync(
        string trackingNumber)
    {
        var shipment = await _unitOfWork.Shipments
            .GetByTrackingNumberAsync(trackingNumber);

        if (shipment is null)
        {
            return null;
        }

        return MapToDto(shipment);
    }

    public async Task<IEnumerable<CreateShipmentDto>> GetByOrderIdAsync(
        int orderId)
    {
        var shipments = await _unitOfWork.Shipments
            .GetByOrderIdAsync(orderId);

        return shipments.Select(MapToDto);
    }

    public async Task<bool> CreateAsync(CreateShipmentDto request)
    {
        if (!_shipmentValidator.IsValid(request))
        {
            return false;
        }

        var order = await _unitOfWork.Orders.GetByIdAsync(request.OrderId);

        if (order is null)
        {
            return false;
        }

        var existingShipment = await _unitOfWork.Shipments
            .GetByOrderIdAsync(request.OrderId);

        if (existingShipment.Any())
        {
            return false;
        }

        var shipment = new Shipment
        {
            TrackingNumber = GenerateTrackingNumber(),
            OrderId = request.OrderId,
            WarehouseId = request.WarehouseId,
            Status = ShipmentStatus.Pending,
            EstimatedDeliveryDate =
                request.EstimatedDeliveryDate
        };

        shipment.TrackingHistory.Add(new ShipmentTracking
        {
            Status = ShipmentStatus.Pending,
            Location = "Warehouse",
            Remarks = "Shipment created"
        });

        await _unitOfWork.Shipments.AddAsync(shipment);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UpdateStatusAsync(
        int id,
        UpdateShipmentStatusDto request)
    {
        if (!_shipmentValidator.IsValid(request))
        {
            return false;
        }

        var shipment = await _unitOfWork.Shipments.GetByIdAsync(id);

        if (shipment is null)
        {
            return false;
        }

        shipment.Status = request.Status;

        if (request.Status == ShipmentStatus.Delivered)
        {
            shipment.DeliveredAt = DateTime.UtcNow;
        }

        shipment.TrackingHistory.Add(new ShipmentTracking
        {
            Status = request.Status,
            Location = request.Location,
            Remarks = request.Remarks
        });

        _unitOfWork.Shipments.Update(shipment);

        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var shipment = await _unitOfWork.Shipments.GetByIdAsync(id);

        if (shipment is null)
        {
            return false;
        }

        _unitOfWork.Shipments.Delete(shipment);

        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    private static string GenerateTrackingNumber()
    {
        return $"TRK-{DateTime.UtcNow:yyyyMMddHHmmssfff}";
    }

    private static CreateShipmentDto MapToDto(Shipment shipment)
    {
        return new CreateShipmentDto
        {
            OrderId = shipment.OrderId,
            WarehouseId = shipment.WarehouseId,
            EstimatedDeliveryDate =
                shipment.EstimatedDeliveryDate
        };
    }
}