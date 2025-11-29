using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopFinance.Domain.Entities;

namespace ShopFinance.Infrastructure.Data.Configurations;

public class StockCountConfiguration : IEntityTypeConfiguration<StockCount>
{
    public void Configure(EntityTypeBuilder<StockCount> builder)
    {
        builder.ToTable("StockCounts");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CountNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.WarehouseId)
            .IsRequired();

        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<int>()
            .HasDefaultValue(StockCountStatus.InProgress);

        builder.Property(x => x.CountDate)
            .IsRequired();

        builder.Property(x => x.CompletedDate);

        builder.Property(x => x.Notes)
            .HasMaxLength(500);

        // Relación con Warehouse
        builder.HasOne(x => x.Warehouse)
            .WithMany(x => x.StockCounts)
            .HasForeignKey(x => x.WarehouseId) // Especificar explícitamente
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        // Relación con Items
        builder.HasMany(x => x.Items)
            .WithOne(x => x.StockCount)
            .HasForeignKey(x => x.StockCountId)
            .OnDelete(DeleteBehavior.Cascade);

        // Índices
        builder.HasIndex(x => x.CountNumber)
            .IsUnique();

        builder.HasIndex(x => x.WarehouseId);

        builder.HasIndex(x => x.Status);

        builder.HasIndex(x => x.CountDate);

        builder.HasIndex(x => new { x.WarehouseId, x.Status });
    }
}