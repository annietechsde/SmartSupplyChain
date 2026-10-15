using SmartSupplyChain.Core.DTOs.Customers;

namespace SmartSupplyChain.Application.Validators;

public class CustomerValidator
{
    public bool IsValid(CreateCustomerDto request)
    {
        if (request is null)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.Email))
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.PhoneNumber))
        {
            return false;
        }

        return true;
    }

    public bool IsValid(UpdateCustomerDto request)
    {
        if (request is null)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.Email))
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.PhoneNumber))
        {
            return false;
        }

        return true;
    }
}