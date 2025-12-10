using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Enums;

namespace ShopFinance.Infrastructure.Data.Configurations;

public class StockTransferConfiguration : IEntityTypeConfiguration<StockTransfer>
{
    public void Configure(EntityTypeBuilder<StockTransfer> builder)
    {
        builder.ToTable("StockTransfers");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.TransferNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.FromWarehouseId)
            .IsRequired();

        builder.Property(x => x.ToWarehouseId)
            .IsRequired();

        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<int>()
            .HasDefaultValue(TransferStatus.Pending);

        builder.Property(x => x.TransferDate)
            .IsRequired();

        builder.Property(x => x.CompletedDate);

        builder.Property(x => x.Notes)
            .HasMaxLength(500);

        // Relaciones con Warehouse (opcionales si tienen query filters)
        builder.HasOne(x => x.FromWarehouse)
            .WithMany()
            .HasForeignKey(x => x.FromWarehouseId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false); // ← OPCIONAL si Warehouse tiene query filter

        builder.HasOne(x => x.ToWarehouse)
            .WithMany()
            .HasForeignKey(x => x.ToWarehouseId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false); // ← OPCIONAL si Warehouse tiene query filter

        // Relación con Items
        builder.HasMany(x => x.Items)
            .WithOne(x => x.StockTransfer)
            .HasForeignKey(x => x.StockTransferId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false); // ← OPCIONAL por el query filter de StockTransfer

        // Índices
        builder.HasIndex(x => x.TransferNumber)
            .IsUnique();

        builder.HasIndex(x => x.FromWarehouseId);

        builder.HasIndex(x => x.ToWarehouseId);

        builder.HasIndex(x => x.Status);

        builder.HasIndex(x => x.TransferDate);
    }
}