using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSupplyChain.Core.Entities;

namespace SmartSupplyChain.Infrastructure.Data.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(customer => customer.Id);

        builder.Property(customer => customer.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(customer => customer.Email)
            .IsRequired()
            .HasMaxLength(150);

        builder.HasIndex(customer => customer.Email)
            .IsUnique();

        builder.Property(customer => customer.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasOne(customer => customer.User)
            .WithMany()
            .HasForeignKey(customer => customer.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(customer => customer.Address)
            .WithMany()
            .HasForeignKey(customer => customer.AddressId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Property(customer => customer.CreatedAt)
            .IsRequired();
    }
}