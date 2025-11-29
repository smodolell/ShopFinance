using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopFinance.Domain.Entities;

namespace ShopFinance.Infrastructure.Data.Configurations;

public class WarehouseProductConfiguration : IEntityTypeConfiguration<WarehouseProduct>
{
    public void Configure(EntityTypeBuilder<WarehouseProduct> builder)
    {
        builder.ToTable("WarehouseProducts");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.WarehouseId)
            .IsRequired();

        builder.Property(x => x.ProductId)
            .IsRequired();

        builder.Property(x => x.StockQuantity)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(x => x.StockMin)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(x => x.StockMax)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(x => x.Location)
            .HasMaxLength(50);

        // Relación con Warehouse
        builder.HasOne(x => x.Warehouse)
            .WithMany(x => x.Products)
            .HasForeignKey(x => x.WarehouseId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relación con Product
        builder.HasOne(x => x.Product)
            .WithMany(x => x.WarehouseProducts)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // Índice único para evitar duplicados de producto en el mismo almacén
        builder.HasIndex(x => new { x.WarehouseId, x.ProductId })
            .IsUnique();

        // Índices para búsquedas
        builder.HasIndex(x => x.StockQuantity);

        builder.HasIndex(x => x.Location)
            .HasFilter("[Location] IS NOT NULL");

        // Índice compuesto para consultas de stock
        builder.HasIndex(x => new { x.WarehouseId, x.StockQuantity });
    }
}
