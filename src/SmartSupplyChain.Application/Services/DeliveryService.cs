using SmartSupplyChain.Application.Interfaces;
using SmartSupplyChain.Core.DTOs.Shipments;
using SmartSupplyChain.Core.Enums;
using SmartSupplyChain.Core.Interfaces;

namespace SmartSupplyChain.Application.Services;

public class DeliveryService : IDeliveryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IShipmentService _shipmentService;

    public DeliveryService(
        IUnitOfWork unitOfWork,
        IShipmentService shipmentService)
    {
        _unitOfWork = unitOfWork;
        _shipmentService = shipmentService;
    }

    public async Task<bool> AssignShipmentAsync(
        int shipmentId,
        int deliveryAgentId)
    {
        if (shipmentId <= 0 || deliveryAgentId <= 0)
        {
            return false;
        }

        var assignment = new Core.Entities.DeliveryAssignment
        {
            ShipmentId = shipmentId,
            DeliveryAgentId = deliveryAgentId
        };

        await _unitOfWork.Delivery.AddAsync(assignment);

        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> AcceptAssignmentAsync(int assignmentId)
    {
        var assignment = await _unitOfWork.Delivery
            .GetByIdAsync(assignmentId);

        if (assignment is null)
        {
            return false;
        }

        assignment.AcceptedAt = DateTime.UtcNow;

        _unitOfWork.Delivery.Update(assignment);

        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> CompleteDeliveryAsync(int assignmentId)
    {
        var assignment = await _unitOfWork.Delivery
            .GetByIdAsync(assignmentId);

        if (assignment is null)
        {
            return false;
        }

        assignment.CompletedAt = DateTime.UtcNow;
        assignment.IsCompleted = true;

        _unitOfWork.Delivery.Update(assignment);

        await _unitOfWork.SaveChangesAsync();

        var shipmentUpdated = await _shipmentService.UpdateStatusAsync(
            assignment.ShipmentId,
            new UpdateShipmentStatusDto
            {
                Status = ShipmentStatus.Delivered,
                Location = "Delivered",
                Remarks = "Delivery completed successfully"
            });

        return shipmentUpdated;
    }

    public async Task<bool> UpdateDeliveryStatusAsync(
        int assignmentId,
        string status)
    {
        var assignment = await _unitOfWork.Delivery
            .GetByIdAsync(assignmentId);

        if (assignment is null || string.IsNullOrWhiteSpace(status))
        {
            return false;
        }

        if (status.Equals(
            "Accepted",
            StringComparison.OrdinalIgnoreCase))
        {
            assignment.AcceptedAt = DateTime.UtcNow;
        }
        else if (status.Equals(
            "Completed",
            StringComparison.OrdinalIgnoreCase))
        {
            assignment.CompletedAt = DateTime.UtcNow;
            assignment.IsCompleted = true;

            await _shipmentService.UpdateStatusAsync(
                assignment.ShipmentId,
                new UpdateShipmentStatusDto
                {
                    Status = ShipmentStatus.Delivered,
                    Location = "Delivered",
                    Remarks = "Delivery completed successfully"
                });
        }
        else
        {
            return false;
        }

        _unitOfWork.Delivery.Update(assignment);

        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<object>> GetAgentAssignmentsAsync(
        int deliveryAgentId)
    {
        if (deliveryAgentId <= 0)
        {
            return Enumerable.Empty<object>();
        }

        var assignments = await _unitOfWork.Delivery
            .GetByAgentIdAsync(deliveryAgentId);

        return assignments.Select(assignment => new
        {
            assignment.Id,
            assignment.ShipmentId,
            assignment.DeliveryAgentId,
            assignment.AssignedAt,
            assignment.AcceptedAt,
            assignment.CompletedAt,
            assignment.IsCompleted
        });
    }
}