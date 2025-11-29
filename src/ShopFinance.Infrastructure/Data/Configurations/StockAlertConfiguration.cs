using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopFinance.Domain.Entities;

namespace ShopFinance.Infrastructure.Data.Configurations;

public class StockAlertConfiguration : IEntityTypeConfiguration<StockAlert>
{
    public void Configure(EntityTypeBuilder<StockAlert> builder)
    {
        builder.ToTable("StockAlerts");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ProductId)
            .IsRequired();

        builder.Property(x => x.WarehouseId)
            .IsRequired();

        builder.Property(x => x.AlertType)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(x => x.CurrentStock)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(x => x.Threshold)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(x => x.Message)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.AlertDate)
            .IsRequired();


  
        builder.HasOne(x => x.Product)
            .WithMany(x => x.StockAlerts) // Navegación inversa
            .HasForeignKey(x => x.ProductId) // Usar la propiedad existente
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);

        builder.HasOne(x => x.Warehouse)
            .WithMany(x => x.StockAlerts) // Navegación inversa  
            .HasForeignKey(x => x.WarehouseId) // Usar la propiedad existente
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);
        // Índices para consultas rápidas
        builder.HasIndex(x => new { x.WarehouseId, x.AlertType });
        builder.HasIndex(x => new { x.ProductId, x.AlertType });
        builder.HasIndex(x => x.AlertDate);
        builder.HasIndex(x => x.AlertType);

        // Índice para alertas no resueltas (podrías agregar un campo IsResolved)
        builder.HasIndex(x => new { x.WarehouseId, x.AlertDate })
            .HasFilter("[AlertType] IN (0,1,2)"); // LowStock, OverStock, OutOfStock
    }
}