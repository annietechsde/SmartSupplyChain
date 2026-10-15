using SmartSupplyChain.Core.DTOs.Auth;

namespace SmartSupplyChain.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);

    Task<bool> UserExistsAsync(string email);
}