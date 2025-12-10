using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopFinance.Domain.Entities;

namespace ShopFinance.Infrastructure.Data.Configurations;
public class CreditRequestConfiguration : IEntityTypeConfiguration<CreditRequest>
{
    public void Configure(EntityTypeBuilder<CreditRequest> builder)
    {
        builder.ToTable("CreditRequests");

        builder.HasKey(cr => cr.Id);

        // Relaciones
        builder.HasOne(cr => cr.Quotation)
            .WithOne()
            .HasForeignKey<CreditRequest>(cr => cr.QuotationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(cr => cr.PhaseState)
            .WithMany()
            .HasForeignKey(cr => cr.PhaseStateId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(cr => cr.Credit)
            .WithOne(c => c.CreditRequest)
            .HasForeignKey<Credit>(c => c.CreditRequestId);

        // Colecciones
        builder.HasMany(cr => cr.Phases)
            .WithOne(crp => crp.CreditRequest)
            .HasForeignKey(crp => crp.CreditRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        // Índices
        builder.HasIndex(cr => cr.QuotationId)
            .IsUnique();

        builder.HasIndex(cr => cr.PhaseStateId);
    }
}
