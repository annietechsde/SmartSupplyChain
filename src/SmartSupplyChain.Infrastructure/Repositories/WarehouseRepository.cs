using Microsoft.EntityFrameworkCore;
using SmartSupplyChain.Core.Entities;
using SmartSupplyChain.Core.Interfaces;
using SmartSupplyChain.Infrastructure.Data;

namespace SmartSupplyChain.Infrastructure.Repositories;

public class WarehouseRepository : IWarehouseRepository
{
    private readonly AppDbContext _context;

    public WarehouseRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Warehouse?> GetByIdAsync(int id)
    {
        return await _context.Warehouses
            .Include(warehouse => warehouse.Address)
            .FirstOrDefaultAsync(warehouse => warehouse.Id == id);
    }

    public async Task<IEnumerable<Warehouse>> GetAllAsync()
    {
        return await _context.Warehouses
            .Include(warehouse => warehouse.Address)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task AddAsync(Warehouse warehouse)
    {
        await _context.Warehouses.AddAsync(warehouse);
    }

    public void Update(Warehouse warehouse)
    {
        _context.Warehouses.Update(warehouse);
    }

    public void Delete(Warehouse warehouse)
    {
        _context.Warehouses.Remove(warehouse);
    }
}