using SmartSupplyChain.Core.DTOs.Users;

namespace SmartSupplyChain.Application.Interfaces;

public interface IUserService
{
    Task<IEnumerable<CreateUserDto>> GetAllAsync();

    Task<CreateUserDto?> GetByIdAsync(int id);

    Task<bool> CreateAsync(CreateUserDto request);

    Task<bool> UpdateAsync(int id, UpdateUserDto request);

    Task<bool> DeleteAsync(int id);
}