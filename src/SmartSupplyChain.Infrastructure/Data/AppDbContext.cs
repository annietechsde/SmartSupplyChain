using Microsoft.EntityFrameworkCore;
using SmartSupplyChain.Core.Entities;

namespace SmartSupplyChain.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<Role> Roles => Set<Role>();

    public DbSet<Customer> Customers => Set<Customer>();

    public DbSet<Address> Addresses => Set<Address>();

    public DbSet<Order> Orders => Set<Order>();

    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    public DbSet<Shipment> Shipments => Set<Shipment>();

    public DbSet<Warehouse> Warehouses => Set<Warehouse>();

    public DbSet<DeliveryAgent> DeliveryAgents => Set<DeliveryAgent>();

    public DbSet<DeliveryAssignment> DeliveryAssignments => Set<DeliveryAssignment>();

    public DbSet<ShipmentTracking> ShipmentTrackings => Set<ShipmentTracking>();

    public DbSet<Payment> Payments => Set<Payment>();

    public DbSet<Notification> Notifications => Set<Notification>();

    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AppDbContext).Assembly);
    }
}