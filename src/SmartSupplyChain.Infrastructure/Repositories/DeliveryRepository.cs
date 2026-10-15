using Microsoft.EntityFrameworkCore;
using SmartSupplyChain.Core.Entities;
using SmartSupplyChain.Core.Interfaces;
using SmartSupplyChain.Infrastructure.Data;

namespace SmartSupplyChain.Infrastructure.Repositories;

public class DeliveryRepository : IDeliveryRepository
{
    private readonly AppDbContext _context;

    public DeliveryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<DeliveryAssignment?> GetByIdAsync(int id)
    {
        return await _context.DeliveryAssignments
            .Include(assignment => assignment.Shipment)
            .Include(assignment => assignment.DeliveryAgent)
            .FirstOrDefaultAsync(assignment => assignment.Id == id);
    }

    public async Task<IEnumerable<DeliveryAssignment>> GetByAgentIdAsync(
        int deliveryAgentId)
    {
        return await _context.DeliveryAssignments
            .Include(assignment => assignment.Shipment)
            .Where(assignment =>
                assignment.DeliveryAgentId == deliveryAgentId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task AddAsync(DeliveryAssignment assignment)
    {
        await _context.DeliveryAssignments.AddAsync(assignment);
    }

    public void Update(DeliveryAssignment assignment)
    {
        _context.DeliveryAssignments.Update(assignment);
    }

    public void Delete(DeliveryAssignment assignment)
    {
        _context.DeliveryAssignments.Remove(assignment);
    }
}