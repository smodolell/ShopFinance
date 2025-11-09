using Ardalis.Result;
using ShopFinance.Domain.Entities;

namespace ShopFinance.Domain.Repositories;
public interface IRoleRepository : IRepository<Role, Guid>
{
    Task<Role?> GetByNameAsync(string roleName, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string roleName, CancellationToken cancellationToken = default);
    Task<bool> IsNameUniqueAsync(string roleName, Guid? excludeId = null, CancellationToken cancellationToken = default);
    Task<List<Role>> GetActiveRolesAsync(CancellationToken cancellationToken = default);

}
