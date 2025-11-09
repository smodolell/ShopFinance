using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShopFinance.Domain.Common.Interfaces;
using ShopFinance.Domain.Entities;
using System.Linq.Expressions;

namespace ShopFinance.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<User, Role, Guid>
{
    public DbSet<Category> Categories { get; set; } = default!;
    public DbSet<Credit> Credits { get; set; } = default!;
    public DbSet<Customer> Customers { get; set; } = default!;
    public DbSet<Frequency> Frequencies { get; set; } = default!;
    public DbSet<Movement> Movements { get; set; } = default!;
    public DbSet<Payment> Payments { get; set; } = default!;
    public DbSet<PaymentApplication> PaymentApplications { get; set; } = default!;
    public DbSet<Product> Products { get; set; } = default!;
    public DbSet<Setting> Settings { get; set; } = default!;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // Configuración automática para entidades auditable (soft delete)
        foreach (var entityType in modelBuilder.Model.GetEntityTypes()
            .Where(e => typeof(IAuditable).IsAssignableFrom(e.ClrType)))
        {
            // Aplicar filtro de query global para soft delete
            var parameter = Expression.Parameter(entityType.ClrType, "e");
            var isDeletedProperty = Expression.Property(parameter, "IsDeleted");
            var equalsFalse = Expression.Equal(isDeletedProperty, Expression.Constant(false));
            var lambda = Expression.Lambda(equalsFalse, parameter);

            modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
        }




    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is IAuditable &&
                       (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entityEntry in entries)
        {
            var auditable = (IAuditable)entityEntry.Entity;

            if (entityEntry.State == EntityState.Added)
            {
                auditable.CreatedAt = DateTime.UtcNow;
            }
            else if (entityEntry.State == EntityState.Modified)
            {
                auditable.UpdatedAt = DateTime.UtcNow;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
