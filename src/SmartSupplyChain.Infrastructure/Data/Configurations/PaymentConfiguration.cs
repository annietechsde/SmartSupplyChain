using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSupplyChain.Core.Entities;

namespace SmartSupplyChain.Infrastructure.Data.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments");

        builder.HasKey(payment => payment.Id);

        builder.Property(payment => payment.Amount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(payment => payment.Method)
            .IsRequired();

        builder.Property(payment => payment.Status)
            .IsRequired();

        builder.Property(payment => payment.TransactionId)
            .HasMaxLength(100);

        builder.HasIndex(payment => payment.TransactionId)
            .IsUnique()
            .HasFilter("[TransactionId] IS NOT NULL");

        builder.Property(payment => payment.CreatedAt)
            .IsRequired();

        builder.HasOne(payment => payment.Order)
            .WithOne(order => order.Payment)
            .HasForeignKey<Payment>(payment => payment.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}