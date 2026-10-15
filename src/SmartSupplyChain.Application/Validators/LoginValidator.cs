using SmartSupplyChain.Core.DTOs.Auth;

namespace SmartSupplyChain.Application.Validators;

public class LoginValidator
{
    public bool IsValid(LoginRequestDto request)
    {
        if (request is null)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.Email))
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            return false;
        }

        return true;
    }
}