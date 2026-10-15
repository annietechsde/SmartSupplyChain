using SmartSupplyChain.Core.DTOs.Orders;

namespace SmartSupplyChain.Application.Validators;

public class OrderValidator
{
    public bool IsValid(CreateOrderDto request)
    {
        if (request is null)
        {
            return false;
        }

        if (request.CustomerId <= 0)
        {
            return false;
        }

        if (request.Items is null || request.Items.Count == 0)
        {
            return false;
        }

        foreach (var item in request.Items)
        {
            if (string.IsNullOrWhiteSpace(item.ProductName))
            {
                return false;
            }

            if (item.Quantity <= 0)
            {
                return false;
            }

            if (item.UnitPrice < 0)
            {
                return false;
            }
        }

        return true;
    }

    public bool IsValid(UpdateOrderDto request)
    {
        if (request is null)
        {
            return false;
        }

        if (!Enum.IsDefined(request.Status))
        {
            return false;
        }

        return true;
    }
}