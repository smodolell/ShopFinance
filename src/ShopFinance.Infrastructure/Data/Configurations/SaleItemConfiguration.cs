using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopFinance.Domain.Entities;

namespace ShopFinance.Infrastructure.Data.Configurations;

public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
{
    public void Configure(EntityTypeBuilder<SaleItem> builder)
    {
        builder.ToTable("SaleItems");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.SaleId)
            .IsRequired();

        builder.Property(x => x.ProductId)
            .IsRequired();

        builder.Property(x => x.Quantity)
            .IsRequired();

        builder.Property(x => x.CostPrice)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(x => x.UnitPrice)
            .IsRequired()
            .HasPrecision(18, 2);

        // Relación con Sale - OPCIONAL por query filter
        builder.HasOne(x => x.Sale)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.SaleId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false); // ← OPCIONAL

        // Relación con Product - OPCIONAL por query filter
        builder.HasOne(x => x.Product)
            .WithMany(x => x.SaleItems)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false); // ← OPCIONAL

        // Índice para evitar duplicados
        builder.HasIndex(x => new { x.SaleId, x.ProductId })
            .IsUnique();
    }
}