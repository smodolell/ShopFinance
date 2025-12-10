using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopFinance.Domain.Entities;

namespace ShopFinance.Infrastructure.Data.Configurations;

public class QuotationPlanConfiguration : IEntityTypeConfiguration<QuotationPlan>
{
    public void Configure(EntityTypeBuilder<QuotationPlan> builder)
    {
        builder.ToTable("QuotationPlans");

        builder.HasKey(qp => qp.Id);

        // Propiedades básicas
        builder.Property(qp => qp.TaxRateId).IsRequired();
        builder.Property(qp => qp.Code).IsRequired().HasMaxLength(50);
        builder.Property(qp => qp.PlanName).IsRequired().HasMaxLength(200);
        builder.Property(qp => qp.Description).HasMaxLength(1000);

        builder.Property(qp => qp.InitialEffectiveDate)
            .IsRequired()
            .HasColumnType("date");

        builder.Property(qp => qp.FinalEffectiveDate)
            .IsRequired()
            .HasColumnType("date");

        builder.Property(qp => qp.IsActive)
            .HasDefaultValue(true);

        // Índice único para código
        builder.HasIndex(qp => qp.Code).IsUnique();

        // Relaciones
        builder.HasOne(qp => qp.TaxRate)
            .WithMany(tr => tr.QuotationPlans)
            .HasForeignKey(qp => qp.TaxRateId);

        builder.HasMany(qp => qp.Frequencies)
            .WithOne(qpf => qpf.QuotationPlan)
            .HasForeignKey(qpf => qpf.QuotationPlanId);

        builder.HasMany(qp => qp.PaymentTerms)
            .WithOne(qppt => qppt.QuotationPlan)
            .HasForeignKey(qppt => qppt.QuotationPlanId);
    }
}