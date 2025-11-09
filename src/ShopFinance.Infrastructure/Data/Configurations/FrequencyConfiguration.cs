using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopFinance.Domain.Entities;

namespace ShopFinance.Infrastructure.Data.Configurations;

public class FrequencyConfiguration : IEntityTypeConfiguration<Frequency>
{
    public void Configure(EntityTypeBuilder<Frequency> builder)
    {
        builder.ToTable("Frequencies");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired();

        builder.HasMany(e => e.Credits)
            .WithOne(e => e.Frequency)
            .HasForeignKey(e => e.FrequencyId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
