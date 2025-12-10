using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopFinance.Domain.Entities;

namespace ShopFinance.Infrastructure.Data.Configurations;

public class QuotationDiscountConfiguration : IEntityTypeConfiguration<QuotationDiscount>
{
    public void Configure(EntityTypeBuilder<QuotationDiscount> builder)
    {
        builder.ToTable("QuotationDiscounts");

        builder.HasKey(d => d.Id);

        // Propiedades
        builder.Property(d => d.Description)
            .HasMaxLength(500);

        builder.Property(d => d.Value)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(d => d.DiscountType)
            .IsRequired()
            .HasConversion<int>();

        // Relación
        builder.HasOne(d => d.Quotation)
            .WithMany(q => q.Discounts)
            .HasForeignKey(d => d.QuotationId)
            .OnDelete(DeleteBehavior.Cascade);

        // Índices
        builder.HasIndex(d => d.QuotationId);
        builder.HasIndex(d => d.DiscountType);
    }
}