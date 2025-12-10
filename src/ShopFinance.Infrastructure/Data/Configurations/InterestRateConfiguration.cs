using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopFinance.Domain.Entities;

namespace ShopFinance.Infrastructure.Data.Configurations;

public class InterestRateConfiguration : IEntityTypeConfiguration<InterestRate>
{
    public void Configure(EntityTypeBuilder<InterestRate> builder)
    {
        // Tabla
        builder.ToTable("InterestRates");

        // Clave primaria int con Identity
        builder.HasKey(ir => ir.Id);

        builder.Property(ir => ir.Id)
            .ValueGeneratedOnAdd() // Auto-incremental
            .IsRequired();

        // Propiedades
        builder.Property(ir => ir.RateName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(ir => ir.AnnualPercentage)
            .IsRequired()
            .HasColumnType("decimal(5,2)") // 99.99%
            .HasPrecision(5, 2);

        builder.Property(ir => ir.EffectiveDate)
            .IsRequired()
            .HasColumnType("date");

        builder.Property(ir => ir.ExpirationDate)
            .HasColumnType("date");

        builder.Property(ir => ir.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        // Ignorar propiedad calculada
        builder.Ignore(ir => ir.MonthlyPercentage);

        // Índices
        builder.HasIndex(ir => ir.RateName)
            .IsUnique()
            .HasDatabaseName("UK_InterestRates_Name");

        builder.HasIndex(ir => ir.IsActive)
            .HasDatabaseName("IX_InterestRates_IsActive");

        builder.HasIndex(ir => ir.EffectiveDate)
            .HasDatabaseName("IX_InterestRates_EffectiveDate");

        // Relaciones
        builder.HasMany(ir => ir.QuotationPlanPaymentTerms)
            .WithOne(qppt => qppt.InterestRate)
            .HasForeignKey(qppt => qppt.InterestRateId)
            .OnDelete(DeleteBehavior.Restrict); // Importante con int

    }


}
