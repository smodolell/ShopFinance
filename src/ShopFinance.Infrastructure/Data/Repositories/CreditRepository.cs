using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Repositories;

namespace ShopFinance.Infrastructure.Data.Repositories;

public class CreditRepository : Repository<Credit, Guid>, ICreditRepository
{
    public CreditRepository(ApplicationDbContext context) : base(context)
    {
    }
}