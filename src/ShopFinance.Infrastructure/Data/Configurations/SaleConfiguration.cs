using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopFinance.Domain.Entities;

namespace ShopFinance.Infrastructure.Data.Configurations;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable("Sales");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.SaleNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.OrderId)
            .IsRequired();

        builder.Property(x => x.SaleDate)
            .IsRequired();

        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(x => x.InvoiceNumber)
            .HasMaxLength(100);

        builder.Property(x => x.PaymentMethod)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(x => x.TotalAmount)
            .IsRequired()
            .HasPrecision(18, 2);

        // Relación con Order
        builder.HasOne(x => x.Order)
            .WithOne(x => x.Sale)
            .HasForeignKey<Sale>(x => x.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relación con Warehouse (si existe)
        builder.HasOne(x => x.Warehouse)
            .WithMany()
            .HasForeignKey(x => x.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        // Relación con Items - CORREGIDA (opcional por query filter)
        builder.HasMany(x => x.Items)
            .WithOne(x => x.Sale)
            .HasForeignKey(x => x.SaleId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false); // ← ESTA ES LA CLAVE: HACER OPCIONAL

        // Índices
        builder.HasIndex(x => x.SaleNumber)
            .IsUnique();

        builder.HasIndex(x => x.InvoiceNumber)
            .IsUnique()
            .HasFilter("[InvoiceNumber] IS NOT NULL");

        builder.HasIndex(x => x.OrderId)
            .IsUnique();

        builder.HasIndex(x => x.SaleDate);

        builder.HasIndex(x => x.Status);
    }
}