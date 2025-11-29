using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopFinance.Domain.Entities;

namespace ShopFinance.Infrastructure.Data.Configurations;

public class StockCountItemConfiguration : IEntityTypeConfiguration<StockCountItem>
{
    public void Configure(EntityTypeBuilder<StockCountItem> builder)
    {
        builder.ToTable("StockCountItems");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.StockCountId)
            .IsRequired();

        builder.Property(x => x.ProductId)
            .IsRequired();

        builder.Property(x => x.SystemQuantity)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(x => x.PhysicalQuantity)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(x => x.Notes)
            .HasMaxLength(500);

        // Ignorar propiedad calculada (se calcula en runtime)
        builder.Ignore(x => x.Variance);


        // Relación con StockCount (opcional por query filter)
        builder.HasOne(x => x.StockCount)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.StockCountId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false); // ← OPCIONAL

        // Relación con Product (opcional por query filter)
        builder.HasOne(x => x.Product)
            .WithMany(x => x.StockCountItems)
            .HasForeignKey(x => x.ProductId) // ← ESPECIFICAR EXPLÍCITAMENTE
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false); // ← OPCIONAL

        // Índice para evitar duplicados de producto en el mismo conteo
        builder.HasIndex(x => new { x.StockCountId, x.ProductId })
            .IsUnique();

        // Índices para consultas de variación
        builder.HasIndex(x => new { x.StockCountId, x.SystemQuantity, x.PhysicalQuantity });
    }
}