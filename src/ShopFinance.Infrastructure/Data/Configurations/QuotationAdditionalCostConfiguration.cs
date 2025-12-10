using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopFinance.Domain.Entities;

namespace ShopFinance.Infrastructure.Data.Configurations;

public class QuotationAdditionalCostConfiguration : IEntityTypeConfiguration<QuotationAdditionalCost>
{
    public void Configure(EntityTypeBuilder<QuotationAdditionalCost> builder)
    {
        builder.ToTable("QuotationAdditionalCosts");

        builder.HasKey(ac => ac.Id);

        // Propiedades
        builder.Property(ac => ac.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(ac => ac.Amount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(ac => ac.CostType)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(ac => ac.IsPercentage)
            .IsRequired();

        // Relación
        builder.HasOne(ac => ac.Quotation)
            .WithMany(q => q.AdditionalCosts)
            .HasForeignKey(ac => ac.QuotationId)
            .OnDelete(DeleteBehavior.Cascade);

        // Índices
        builder.HasIndex(ac => ac.QuotationId);
        builder.HasIndex(ac => ac.CostType);
    }
}
