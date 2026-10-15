using Microsoft.EntityFrameworkCore;
using SmartSupplyChain.Core.Entities;
using SmartSupplyChain.Core.Enums;
using SmartSupplyChain.Infrastructure.Data;
using SmartSupplyChain.Infrastructure.Security;

namespace SmartSupplyChain.Infrastructure.Data.Seed;

public class DatabaseSeeder
{
    private readonly AppDbContext _context;
    private readonly PasswordHasher _passwordHasher;

    public DatabaseSeeder(
        AppDbContext context,
        PasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task SeedAsync()
    {
        await _context.Database.MigrateAsync();

        if (!await _context.Users.AnyAsync())
        {
            var adminUser = new User
            {
                FirstName = "System",
                LastName = "Admin",
                Email = "admin@smartsupplychain.com",
                PasswordHash = _passwordHasher.HashPassword("Admin@123"),
                Role = UserRole.Admin,
                IsActive = true
            };

            _context.Users.Add(adminUser);
            await _context.SaveChangesAsync();
        }
    }
}