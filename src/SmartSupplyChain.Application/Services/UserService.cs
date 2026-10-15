using SmartSupplyChain.Application.Interfaces;
using SmartSupplyChain.Core.DTOs.Users;
using SmartSupplyChain.Core.Entities;
using SmartSupplyChain.Core.Interfaces;
using SmartSupplyChain.Infrastructure.Security;

namespace SmartSupplyChain.Application.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly PasswordHasher _passwordHasher;

    public UserService(
        IUnitOfWork unitOfWork,
        PasswordHasher passwordHasher)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    public async Task<IEnumerable<CreateUserDto>> GetAllAsync()
    {
        var users = await _unitOfWork.Users.GetAllAsync();

        return users.Select(user => new CreateUserDto
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Role = user.Role
        });
    }

    public async Task<CreateUserDto?> GetByIdAsync(int id)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);

        if (user is null)
        {
            return null;
        }

        return new CreateUserDto
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Role = user.Role
        };
    }

    public async Task<bool> CreateAsync(CreateUserDto request)
    {
        var existingUser = await _unitOfWork.Users
            .GetByEmailAsync(request.Email);

        if (existingUser is not null)
        {
            return false;
        }

        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PasswordHash = _passwordHasher.HashPassword(request.Password),
            Role = request.Role,
            IsActive = true
        };

        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UpdateAsync(
        int id,
        UpdateUserDto request)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);

        if (user is null)
        {
            return false;
        }

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Email = request.Email;
        user.Role = request.Role;
        user.IsActive = request.IsActive;
        user.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Users.Update(user);

        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);

        if (user is null)
        {
            return false;
        }

        _unitOfWork.Users.Delete(user);

        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}