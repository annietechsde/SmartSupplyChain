using SmartSupplyChain.Core.DTOs.Auth;

namespace SmartSupplyChain.Core.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);

    Task<bool> UserExistsAsync(string email);
}