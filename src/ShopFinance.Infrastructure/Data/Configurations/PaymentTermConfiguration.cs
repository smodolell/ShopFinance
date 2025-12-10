using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopFinance.Domain.Entities;

namespace ShopFinance.Infrastructure.Data.Configurations;

public class PaymentTermConfiguration : IEntityTypeConfiguration<PaymentTerm>
{
    public void Configure(EntityTypeBuilder<PaymentTerm> builder)
    {
        // Tabla
        builder.ToTable("PaymentTerms");

        // Clave primaria
        builder.HasKey(pt => pt.Id);

        // Propiedades
        builder.Property(pt => pt.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(pt => pt.Code)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(pt => pt.NumberOfPayments)
            .IsRequired();

        // Índices
        builder.HasIndex(pt => pt.Code)
            .IsUnique();

        builder.HasIndex(pt => pt.NumberOfPayments);

     
        // Relaciones
        builder.HasMany(pt => pt.QuotationPlanPaymentTerms)
            .WithOne(qppt => qppt.PaymentTerm)
            .HasForeignKey(qppt => qppt.PaymentTermId)
            .OnDelete(DeleteBehavior.Restrict);

        // Seed data para MVP
        SeedData(builder);
    }

    private void SeedData(EntityTypeBuilder<PaymentTerm> builder)
    {
        builder.HasData(
            new PaymentTerm { Id = 1, Name = "Un Pago", Code = "TERM_1", NumberOfPayments = 1 },
            new PaymentTerm { Id = 2, Name = "3 Pagos", Code = "TERM_3", NumberOfPayments = 3 },
            new PaymentTerm { Id = 3, Name = "6 Pagos", Code = "TERM_6", NumberOfPayments = 6 },
            new PaymentTerm { Id = 4, Name = "12 Pagos", Code = "TERM_12", NumberOfPayments = 12 },
            new PaymentTerm { Id = 5, Name = "18 Pagos", Code = "TERM_18", NumberOfPayments = 18 },
            new PaymentTerm { Id = 6, Name = "24 Pagos", Code = "TERM_24", NumberOfPayments = 24 },
            new PaymentTerm { Id = 7, Name = "36 Pagos", Code = "TERM_36", NumberOfPayments = 36 }
        );
    }
}
