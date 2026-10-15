using Microsoft.EntityFrameworkCore;
using SmartSupplyChain.Core.Entities;
using SmartSupplyChain.Core.Interfaces;
using SmartSupplyChain.Infrastructure.Data;

namespace SmartSupplyChain.Infrastructure.Repositories;

public class ShipmentRepository : IShipmentRepository
{
    private readonly AppDbContext _context;

    public ShipmentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Shipment?> GetByIdAsync(int id)
    {
        return await _context.Shipments
            .Include(shipment => shipment.Order)
            .Include(shipment => shipment.Warehouse)
            .Include(shipment => shipment.TrackingHistory)
            .FirstOrDefaultAsync(shipment => shipment.Id == id);
    }

    public async Task<Shipment?> GetByTrackingNumberAsync(string trackingNumber)
    {
        return await _context.Shipments
            .Include(shipment => shipment.Order)
            .Include(shipment => shipment.Warehouse)
            .Include(shipment => shipment.TrackingHistory)
            .FirstOrDefaultAsync(
                shipment => shipment.TrackingNumber == trackingNumber);
    }

    public async Task<IEnumerable<Shipment>> GetAllAsync()
    {
        return await _context.Shipments
            .Include(shipment => shipment.Order)
            .Include(shipment => shipment.Warehouse)
            .Include(shipment => shipment.TrackingHistory)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Shipment>> GetByOrderIdAsync(int orderId)
    {
        return await _context.Shipments
            .Include(shipment => shipment.TrackingHistory)
            .Where(shipment => shipment.OrderId == orderId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task AddAsync(Shipment shipment)
    {
        await _context.Shipments.AddAsync(shipment);
    }

    public void Update(Shipment shipment)
    {
        _context.Shipments.Update(shipment);
    }

    public void Delete(Shipment shipment)
    {
        _context.Shipments.Remove(shipment);
    }
}