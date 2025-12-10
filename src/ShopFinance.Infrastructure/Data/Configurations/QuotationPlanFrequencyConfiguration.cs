
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopFinance.Domain.Entities;

public class QuotationPlanFrequencyConfiguration : IEntityTypeConfiguration<QuotationPlanFrequency>
{
    public void Configure(EntityTypeBuilder<QuotationPlanFrequency> builder)
    {
        builder.ToTable("QuotationPlanFrequencies");

        builder.HasKey(qpf => qpf.Id);

        // Propiedades básicas
        builder.Property(qpf => qpf.QuotationPlanId).IsRequired();
        builder.Property(qpf => qpf.FrequencyId).IsRequired();
        builder.Property(qpf => qpf.IsDefault).HasDefaultValue(false);
        builder.Property(qpf => qpf.Order).HasDefaultValue(0);

        // Índice único para evitar duplicados (lo más importante)
        builder.HasIndex(qpf => new { qpf.QuotationPlanId, qpf.FrequencyId })
            .IsUnique();

        // Relaciones básicas
        builder.HasOne(qpf => qpf.QuotationPlan)
            .WithMany(qp => qp.Frequencies)
            .HasForeignKey(qpf => qpf.QuotationPlanId);

        builder.HasOne(qpf => qpf.Frequency)
            .WithMany(f => f.QuotationPlanFrequencies)
            .HasForeignKey(qpf => qpf.FrequencyId);
    }
}