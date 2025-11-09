using ShopFinance.Domain.Entities;

namespace ShopFinance.Domain.Repositories;

public interface ISettingRepository : IRepository<Setting, Guid>
{

    Task<Setting?> GetByKeyAsync(string key);
    Task<bool> KeyExistsAsync(string key);
}