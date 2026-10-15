
using SmartSupplyChain.Core.Entities;
using SmartSupplyChain.Core.Interfaces;
using SmartSupplyChain.Infrastructure.Data;

namespace SmartSupplyChain.Infrastructure.Repositories;

public class AddressRepository : IAddressRepository
{
    private readonly AppDbContext _context;

    public AddressRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Address address)
    {
        await _context.Addresses.AddAsync(address);
    }
}
