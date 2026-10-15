using Moq;
using SmartSupplyChain.Application.Services;
using SmartSupplyChain.Application.Validators;
using SmartSupplyChain.Core.DTOs.Auth;
using SmartSupplyChain.Core.Entities;
using SmartSupplyChain.Core.Interfaces;
using SmartSupplyChain.Infrastructure.Security;

namespace SmartSupplyChain.Tests.Services;

public class AuthServiceTests
{
    [Fact]
    public async Task LoginAsync_ReturnsNull_WhenUserDoesNotExist()
    {
        var unitOfWork = new Mock<IUnitOfWork>();
        var jwtService = new Mock<JwtService>(new object[] { null! });
        var passwordHasher = new PasswordHasher();
        var validator = new LoginValidator();

        unitOfWork
            .Setup(unit => unit.Users.GetByEmailAsync(
                It.IsAny<string>()))
            .ReturnsAsync((User?)null);

        var service = new AuthService(
            unitOfWork.Object,
            jwtService.Object,
            passwordHasher,
            validator);

        var request = new LoginRequestDto
        {
            Email = "unknown@example.com",
            Password = "Password123"
        };

        var result = await service.LoginAsync(request);

        Assert.Null(result);
    }

    [Fact]
    public async Task UserExistsAsync_ReturnsTrue_WhenUserExists()
    {
        var unitOfWork = new Mock<IUnitOfWork>();
        var jwtService = new Mock<JwtService>(new object[] { null! });
        var passwordHasher = new PasswordHasher();
        var validator = new LoginValidator();

        var user = new User
        {
            Id = 1,
            FirstName = "Test",
            LastName = "User",
            Email = "test@example.com",
            PasswordHash = passwordHasher.HashPassword("Password123"),
            IsActive = true
        };

        unitOfWork
            .Setup(unit => unit.Users.GetByEmailAsync(
                user.Email))
            .ReturnsAsync(user);

        var service = new AuthService(
            unitOfWork.Object,
            jwtService.Object,
            passwordHasher,
            validator);

        var result = await service.UserExistsAsync(user.Email);

        Assert.True(result);
    }
}