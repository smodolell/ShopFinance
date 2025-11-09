using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ShopFinance.Infrastructure.Data.Configurations;

public class UserLoginConfiguration : IEntityTypeConfiguration<IdentityUserLogin<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityUserLogin<Guid>> builder)
    {
        builder.ToTable("UserLogins");

        // Configurar clave primaria compuesta si es necesario
        builder.HasKey(l => new { l.LoginProvider, l.ProviderKey });
    }
}
