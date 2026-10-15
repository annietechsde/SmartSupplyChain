namespace SmartSupplyChain.Core.Interfaces;

public interface IUnitOfWork
{
    IUserRepository Users { get; }
    ICustomerRepository Customers { get; }
    IAddressRepository Addresses { get; }
    IOrderRepository Orders { get; }
    IShipmentRepository Shipments { get; }
    IWarehouseRepository Warehouses { get; }
    IDeliveryRepository Delivery { get; }

    Task<int> SaveChangesAsync();

    Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> operation);
}