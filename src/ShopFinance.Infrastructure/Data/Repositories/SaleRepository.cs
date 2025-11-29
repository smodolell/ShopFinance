using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Repositories;

namespace ShopFinance.Infrastructure.Data.Repositories;

public class SaleRepository : Repository<Sale, Guid>, ISaleRepository
{
    public SaleRepository(ApplicationDbContext context) : base(context)
    {
    }
}
