using Microsoft.EntityFrameworkCore;
using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Repositories;

namespace ShopFinance.Infrastructure.Data.Repositories;

internal class SettingRepository : Repository<Setting, Guid>, ISettingRepository
{
    public SettingRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Setting?> GetByKeyAsync(string key)
    {
        return await _context.Settings
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Key == key);
    }

    public async Task<bool> KeyExistsAsync(string key)
    {
        return await _context.Settings
            .AsNoTracking()
            .AnyAsync(s => s.Key == key);
    }
}
