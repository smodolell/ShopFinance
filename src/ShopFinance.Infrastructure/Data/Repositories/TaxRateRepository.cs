using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Repositories;

namespace ShopFinance.Infrastructure.Data.Repositories;

public class TaxRateRepository : Repository<TaxRate, int>, ITaxRateRepository
{
    public TaxRateRepository(ApplicationDbContext context) : base(context)
    {
    }
}
