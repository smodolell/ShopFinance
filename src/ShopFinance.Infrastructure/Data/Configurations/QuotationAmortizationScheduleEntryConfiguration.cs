using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopFinance.Domain.Entities;

namespace ShopFinance.Infrastructure.Data.Configurations;

public class QuotationAmortizationScheduleEntryConfiguration : IEntityTypeConfiguration<QuotationAmortizationScheduleEntry>
{
    public void Configure(EntityTypeBuilder<QuotationAmortizationScheduleEntry> builder)
    {
        builder.ToTable("QuotationAmortizationScheduleEntries");

        builder.HasKey(ase => ase.Id);

        // Propiedades
        builder.Property(ase => ase.Principal)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(ase => ase.Interest)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(ase => ase.InterestTax)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(ase => ase.TotalDue)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(ase => ase.PeriodNumber)
            .IsRequired();

        builder.Property(ase => ase.DueDate)
            .IsRequired();

        // Relación
        builder.HasOne(ase => ase.Quotation)
            .WithMany(q => q.AmortizationScheduleEntries)
            .HasForeignKey(ase => ase.QuotationId)
            .OnDelete(DeleteBehavior.Cascade);

        // Índices compuestos
        builder.HasIndex(ase => new { ase.QuotationId, ase.PeriodNumber })
            .IsUnique();

        builder.HasIndex(ase => ase.DueDate);
    }
}