using SmartSupplyChain.Core.Enums;

namespace SmartSupplyChain.Core.DTOs.Users;

public class UpdateUserDto
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public UserRole Role { get; set; }

    public bool IsActive { get; set; }
}