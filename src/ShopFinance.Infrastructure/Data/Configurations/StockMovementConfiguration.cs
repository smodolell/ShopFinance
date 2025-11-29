using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopFinance.Domain.Entities;

namespace ShopFinance.Infrastructure.Data.Configurations;

public class StockMovementConfiguration : IEntityTypeConfiguration<StockMovement>
{
    public void Configure(EntityTypeBuilder<StockMovement> builder)
    {
        builder.ToTable("StockMovements");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.WarehouseId)
            .IsRequired();

        builder.Property(x => x.ProductId)
            .IsRequired();

        builder.Property(x => x.ReferenceId);

        builder.Property(x => x.MovementType)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(x => x.Source)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(x => x.Quantity)
            .IsRequired();

        builder.Property(x => x.PreviousStock)
            .IsRequired();

        builder.Property(x => x.NewStock)
            .IsRequired();

        builder.Property(x => x.Notes)
            .HasMaxLength(500);

        builder.Property(x => x.MovementDate)
            .IsRequired();

        // Relación con Warehouse
        builder.HasOne(x => x.Warehouse)
            .WithMany(x => x.StockMovements)
            .HasForeignKey(x => x.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relación con Product
        builder.HasOne(x => x.Product)
            .WithMany(x => x.StockMovements)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // Índices para búsquedas rápidas
        builder.HasIndex(x => new { x.WarehouseId, x.MovementDate });

        builder.HasIndex(x => new { x.ProductId, x.MovementDate });

        builder.HasIndex(x => new { x.ReferenceId, x.Source })
            .HasFilter("[ReferenceId] IS NOT NULL");

        builder.HasIndex(x => x.MovementType);

        builder.HasIndex(x => x.Source);

        builder.HasIndex(x => x.MovementDate);
    }
}