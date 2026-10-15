using SmartSupplyChain.Core.DTOs.Shipments;

namespace SmartSupplyChain.Application.Validators;

public class ShipmentValidator
{
    public bool IsValid(CreateShipmentDto request)
    {
        if (request is null)
        {
            return false;
        }

        if (request.OrderId <= 0)
        {
            return false;
        }

        if (request.WarehouseId.HasValue && request.WarehouseId.Value <= 0)
        {
            return false;
        }

        return true;
    }

    public bool IsValid(UpdateShipmentStatusDto request)
    {
        if (request is null)
        {
            return false;
        }

        if (!Enum.IsDefined(request.Status))
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.Location))
        {
            return false;
        }

        return true;
    }
}