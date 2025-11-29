using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopFinance.Domain.Entities;

namespace ShopFinance.Infrastructure.Data.Configurations;

public class WarehouseSummaryConfiguration : IEntityTypeConfiguration<WarehouseSummary>
{
    public void Configure(EntityTypeBuilder<WarehouseSummary> builder)
    {
        builder.ToTable("WarehouseSummaries");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.WarehouseId)
            .IsRequired();

        builder.Property(x => x.TotalProducts)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(x => x.LowStockProducts)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(x => x.OutOfStockProducts)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(x => x.TotalInventoryValue)
            .IsRequired()
            .HasPrecision(18, 2)
            .HasDefaultValue(0);

        builder.Property(x => x.TotalMovementsToday)
            .IsRequired()
            .HasDefaultValue(0);

        // Relación con Warehouse (opcional, si quieres navegación)
        builder.HasOne<Warehouse>()
            .WithOne()
            .HasForeignKey<WarehouseSummary>(x => x.WarehouseId)
            .OnDelete(DeleteBehavior.Cascade);

        // Índice único por warehouse
        builder.HasIndex(x => x.WarehouseId)
            .IsUnique();
    }
}