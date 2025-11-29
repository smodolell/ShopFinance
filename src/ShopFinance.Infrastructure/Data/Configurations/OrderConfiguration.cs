using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopFinance.Domain.Entities;

namespace ShopFinance.Infrastructure.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.OrderNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.OrderDate)
            .IsRequired();

        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(x => x.TotalAmount)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(x => x.Notes)
            .HasMaxLength(500);

        builder.Property(x => x.RequiredDate);

        // Relación con Customer
        builder.HasOne(x => x.Customer)
            .WithMany()
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relación con OrderItems
        builder.HasMany(x => x.Items)
                  .WithOne(x => x.Order)
                  .HasForeignKey(x => x.OrderId)
                  .OnDelete(DeleteBehavior.Cascade)
                  .IsRequired(false); // ← Hacer opcional

        // Relación 1:1 con Sale
        builder.HasOne(x => x.Sale)
            .WithOne(x => x.Order)
            .HasForeignKey<Sale>(x => x.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        // Índices
        builder.HasIndex(x => x.OrderNumber)
            .IsUnique();

        builder.HasIndex(x => x.CustomerId);
        builder.HasIndex(x => x.OrderDate);
        builder.HasIndex(x => x.Status);
    }
}
