using Microsoft.EntityFrameworkCore;
using SmartSupplyChain.Core.Entities;
using SmartSupplyChain.Core.Interfaces;
using SmartSupplyChain.Infrastructure.Data;

namespace SmartSupplyChain.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Order?> GetByIdAsync(int id)
    {
        return await _context.Orders
            .Include(order => order.Customer)
            .Include(order => order.OrderItems)
            .Include(order => order.Shipment)
            .Include(order => order.Payment)
            .FirstOrDefaultAsync(order => order.Id == id);
    }

    
    public async Task<Order?> GetByExternalOrderIdAsync(string externalOrderId)
    {
        return await _context.Orders
            .Include(order => order.Customer)
            .Include(order => order.OrderItems)
            .FirstOrDefaultAsync(
                order => order.ExternalOrderId == externalOrderId);
    }


    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        return await _context.Orders
            .Include(order => order.Customer)
            .Include(order => order.OrderItems)
            .Include(order => order.Shipment)
            .Include(order => order.Payment)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetByCustomerIdAsync(int customerId)
    {
        return await _context.Orders
            .Include(order => order.OrderItems)
            .Include(order => order.Shipment)
            .Include(order => order.Payment)
            .Where(order => order.CustomerId == customerId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task AddAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
    }

    public void Update(Order order)
    {
        _context.Orders.Update(order);
    }

    public void Delete(Order order)
    {
        _context.Orders.Remove(order);
    }
}