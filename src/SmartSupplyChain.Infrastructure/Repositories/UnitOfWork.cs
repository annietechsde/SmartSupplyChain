using Microsoft.EntityFrameworkCore;
using SmartSupplyChain.Core.Interfaces;
using SmartSupplyChain.Infrastructure.Data;

namespace SmartSupplyChain.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public IUserRepository Users { get; }
    public ICustomerRepository Customers { get; }
    public IAddressRepository Addresses { get; }
    public IOrderRepository Orders { get; }
    public IShipmentRepository Shipments { get; }
    public IWarehouseRepository Warehouses { get; }
    public IDeliveryRepository Delivery { get; }

    public UnitOfWork(
        AppDbContext context,
        IUserRepository users,
        ICustomerRepository customers,
        IAddressRepository addresses,
        IOrderRepository orders,
        IShipmentRepository shipments,
        IWarehouseRepository warehouses,
        IDeliveryRepository delivery)
    {
        _context = context;
        Users = users;
        Customers = customers;
        Addresses = addresses;
        Orders = orders;
        Shipments = shipments;
        Warehouses = warehouses;
        Delivery = delivery;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task<T> ExecuteInTransactionAsync<T>(
        Func<Task<T>> operation)
    {
        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        try
        {
            var result = await operation();

            await transaction.CommitAsync();

            return result;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}