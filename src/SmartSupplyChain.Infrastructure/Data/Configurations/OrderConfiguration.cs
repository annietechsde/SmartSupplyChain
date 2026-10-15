
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSupplyChain.Core.Entities;

namespace SmartSupplyChain.Infrastructure.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(order => order.Id);

        builder.Property(order => order.OrderNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(order => order.OrderNumber)
            .IsUnique();

        // Prevent duplicate external order IDs.
        // Multiple orders may have a NULL ExternalOrderId.
        builder.HasIndex(order => order.ExternalOrderId)
            .IsUnique()
            .HasFilter("[ExternalOrderId] IS NOT NULL");

        builder.Property(order => order.Status)
            .IsRequired();

        builder.Property(order => order.TotalAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(order => order.OrderDate)
            .IsRequired();

        builder.HasOne(order => order.Customer)
            .WithMany()
            .HasForeignKey(order => order.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(order => order.DeliveryAddress)
            .WithMany()
            .HasForeignKey(order => order.DeliveryAddressId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(order => order.OrderItems)
            .WithOne(item => item.Order)
            .HasForeignKey(item => item.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(order => order.Shipment)
            .WithOne(shipment => shipment.Order)
            .HasForeignKey<Shipment>(shipment => shipment.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(order => order.Payment)
            .WithOne(payment => payment.Order)
            .HasForeignKey<Payment>(payment => payment.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
