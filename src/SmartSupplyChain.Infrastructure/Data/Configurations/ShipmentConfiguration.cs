using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSupplyChain.Core.Entities;

namespace SmartSupplyChain.Infrastructure.Data.Configurations;

public class ShipmentConfiguration : IEntityTypeConfiguration<Shipment>
{
    public void Configure(EntityTypeBuilder<Shipment> builder)
    {
        builder.ToTable("Shipments");

        builder.HasKey(shipment => shipment.Id);

        builder.Property(shipment => shipment.TrackingNumber)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(shipment => shipment.TrackingNumber)
            .IsUnique();

        builder.Property(shipment => shipment.Status)
            .IsRequired();

        builder.HasOne(shipment => shipment.Order)
            .WithOne(order => order.Shipment)
            .HasForeignKey<Shipment>(shipment => shipment.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(shipment => shipment.Warehouse)
            .WithMany(warehouse => warehouse.Shipments)
            .HasForeignKey(shipment => shipment.WarehouseId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(shipment => shipment.TrackingHistory)
            .WithOne(tracking => tracking.Shipment)
            .HasForeignKey(tracking => tracking.ShipmentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}