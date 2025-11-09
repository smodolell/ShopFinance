using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopFinance.Domain.Entities;

namespace ShopFinance.Infrastructure.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        // Table name
        builder.ToTable("Products");

        // Primary Key
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Code)
        .IsRequired()
        .HasMaxLength(50);

        builder.Property(p => p.CodeSku)
            .IsRequired()
            .HasMaxLength(100);


        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Description)
            .HasMaxLength(1000);

        builder.Property(p => p.CostPrice)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(p => p.SalePrice)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(p => p.Stock)
            .IsRequired();

        builder.Property(p => p.StockMin)
            .IsRequired();

        builder.Property(p => p.State)
            .IsRequired()
            .HasConversion<string>() // Si ProductState es un enum, lo convierte a string
            .HasMaxLength(50);

        // Relationships
        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict); // o DeleteBehavior.Cascade según tu necesidad

        // Indexes únicos para códigos
        builder.HasIndex(p => p.Code)
            .IsUnique();

        builder.HasIndex(p => p.CodeSku)
            .IsUnique();

        // Indexes
        builder.HasIndex(p => p.Name);
        builder.HasIndex(p => p.CategoryId);
        builder.HasIndex(p => p.State);

        // Query filters (opcional)
        // builder.HasQueryFilter(p => p.State == ProductState.Active); // si quieres filtrar productos activos
    }
}
