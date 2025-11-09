using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopFinance.Domain.Entities;

namespace ShopFinance.Infrastructure.Data.Configurations;

public class CreditConfiguration : IEntityTypeConfiguration<Credit>
{
    public void Configure(EntityTypeBuilder<Credit> builder)
    {
        builder.ToTable("Credits");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.FrequencyId)
            .IsRequired();

        builder.Property(e => e.CreditKey)
            .IsRequired();

        builder.Property(e => e.Rate)
            .IsRequired();

        builder.Property(e => e.TaxRate)
            .IsRequired();

        builder.Property(e => e.Term)
            .IsRequired();

        builder.Property(e => e.PrincipalAmount)
            .IsRequired();

        builder.Property(e => e.FinancedAmount)
            .IsRequired();

        builder.Property(e => e.CreditState)
            .IsRequired();

        // Relationships

        builder.HasOne(e => e.Frequency)
            .WithMany(e => e.Credits)
            .HasForeignKey(e => e.FrequencyId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
