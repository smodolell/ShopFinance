using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopFinance.Domain.Entities;

namespace ShopFinance.Infrastructure.Data.Configurations;

public class TaxRateConfiguration : IEntityTypeConfiguration<TaxRate>
{
    public void Configure(EntityTypeBuilder<TaxRate> builder)
    {
        builder.ToTable("TaxRates");

        builder.HasKey(tr => tr.Id);

        builder.Property(tr => tr.Id)
            .ValueGeneratedOnAdd();

        builder.Property(tr => tr.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(tr => tr.Code)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(tr => tr.Percentage)
            .IsRequired()
            .HasColumnType("decimal(5,2)")
            .HasPrecision(5, 2);

        builder.Property(tr => tr.EffectiveDate)
            .IsRequired()
            .HasColumnType("date");

        builder.Property(tr => tr.ExpirationDate)
            .HasColumnType("date");

        builder.Property(tr => tr.IsActive)
            .HasDefaultValue(true);

        // Índices
        builder.HasIndex(tr => tr.Code).IsUnique();
        builder.HasIndex(tr => tr.IsActive);
        builder.HasIndex(tr => tr.EffectiveDate);

        // Relación
        builder.HasMany(tr => tr.QuotationPlans)
            .WithOne(qp => qp.TaxRate)
            .HasForeignKey(qp => qp.TaxRateId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}