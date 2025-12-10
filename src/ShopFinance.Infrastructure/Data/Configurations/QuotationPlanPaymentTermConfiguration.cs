
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopFinance.Domain.Entities;

public class QuotationPlanPaymentTermConfiguration : IEntityTypeConfiguration<QuotationPlanPaymentTerm>
{
    public void Configure(EntityTypeBuilder<QuotationPlanPaymentTerm> builder)
    {
        builder.ToTable("QuotationPlanPaymentTerms");

        builder.HasKey(qppt => qppt.Id);

        // Propiedades básicas
        builder.Property(qppt => qppt.QuotationPlanId).IsRequired();
        builder.Property(qppt => qppt.PaymentTermId).IsRequired();
        builder.Property(qppt => qppt.InterestRateId).IsRequired();

        builder.Property(qppt => qppt.SpecialRateOverride)
            .HasColumnType("decimal(5,2)")
            .HasPrecision(5, 2);

        builder.Property(qppt => qppt.IsPromotional)
            .HasDefaultValue(false);

        builder.Property(qppt => qppt.PromotionEndDate)
            .HasColumnType("date");

        builder.Property(qppt => qppt.Order)
            .HasDefaultValue(0);

        // Índice único esencial (evitar duplicados)
        builder.HasIndex(qppt => new { qppt.QuotationPlanId, qppt.PaymentTermId })
            .IsUnique();

        // Relaciones
        builder.HasOne(qppt => qppt.QuotationPlan)
            .WithMany(qp => qp.PaymentTerms)
            .HasForeignKey(qppt => qppt.QuotationPlanId);

        builder.HasOne(qppt => qppt.PaymentTerm)
            .WithMany(pt => pt.QuotationPlanPaymentTerms)
            .HasForeignKey(qppt => qppt.PaymentTermId);

        builder.HasOne(qppt => qppt.InterestRate)
            .WithMany(ir => ir.QuotationPlanPaymentTerms)
            .HasForeignKey(qppt => qppt.InterestRateId);
    }
}