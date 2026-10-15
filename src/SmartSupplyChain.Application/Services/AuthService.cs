using SmartSupplyChain.Application.Validators;
using SmartSupplyChain.Core.DTOs.Auth;
using SmartSupplyChain.Core.Interfaces;
using SmartSupplyChain.Infrastructure.Security;

namespace SmartSupplyChain.Application.Services;

public class AuthService : Application.Interfaces.IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly JwtService _jwtService;
    private readonly PasswordHasher _passwordHasher;
    private readonly LoginValidator _loginValidator;

    public AuthService(
        IUnitOfWork unitOfWork,
        JwtService jwtService,
        PasswordHasher passwordHasher,
        LoginValidator loginValidator)
    {
        _unitOfWork = unitOfWork;
        _jwtService = jwtService;
        _passwordHasher = passwordHasher;
        _loginValidator = loginValidator;
    }

    public async Task<LoginResponseDto?> LoginAsync(
        LoginRequestDto request)
    {
        if (!_loginValidator.IsValid(request))
        {
            return null;
        }

        var user = await _unitOfWork.Users
            .GetByEmailAsync(request.Email);

        if (user is null)
        {
            return null;
        }

        if (!user.IsActive)
        {
            return null;
        }

        var passwordValid = _passwordHasher.VerifyPassword(
            request.Password,
            user.PasswordHash);

        if (!passwordValid)
        {
            return null;
        }

        var token = _jwtService.GenerateToken(user);

        return new LoginResponseDto
        {
            UserId = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Role = user.Role.ToString(),
            Token = token
        };
    }

    public async Task<bool> UserExistsAsync(string email)
    {
        var user = await _unitOfWork.Users.GetByEmailAsync(email);

        return user is not null;
    }
}