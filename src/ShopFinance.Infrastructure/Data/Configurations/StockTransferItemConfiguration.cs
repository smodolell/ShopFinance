using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopFinance.Domain.Entities;

namespace ShopFinance.Infrastructure.Data.Configurations;

public class StockTransferItemConfiguration : IEntityTypeConfiguration<StockTransferItem>
{
    public void Configure(EntityTypeBuilder<StockTransferItem> builder)
    {
        builder.ToTable("StockTransferItems");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.StockTransferId)
            .IsRequired();

        builder.Property(x => x.ProductId)
            .IsRequired();

        builder.Property(x => x.Quantity)
            .IsRequired();

        builder.Property(x => x.QuantitySent)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(x => x.QuantityReceived)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(x => x.Notes)
            .HasMaxLength(500);

        // Relación con StockTransfer (OPCIONAL por query filter)
        builder.HasOne(x => x.StockTransfer)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.StockTransferId) // ← ESPECIFICAR EXPLÍCITAMENTE
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false); // ← OPCIONAL

        // Relación con Product (OPCIONAL por query filter)
        builder.HasOne(x => x.Product)
            .WithMany(x => x.StockTransferItems)
            .HasForeignKey(x => x.ProductId) // ← ESPECIFICAR EXPLÍCITAMENTE
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false); // ← OPCIONAL

        // Índice único
        builder.HasIndex(x => new { x.StockTransferId, x.ProductId })
            .IsUnique();
    }
}