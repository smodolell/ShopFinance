using ShopFinance.Domain.Repositories;

namespace ShopFinance.Domain.Common.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ICategoryRepository Categories { get; }
    ICustomerRepository Customers { get; }
    ICreditRepository Credits { get; }
    IFrequencyRepository Frequencies { get; }
    IProductRepository Products { get; }
    IUserRepository Users { get; }
    IRoleRepository Roles { get; }
    ISettingRepository Settings { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
}