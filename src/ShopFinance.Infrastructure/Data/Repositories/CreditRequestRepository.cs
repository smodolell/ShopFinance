using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Repositories;

namespace ShopFinance.Infrastructure.Data.Repositories;

public class CreditRequestRepository : Repository<CreditRequest, Guid>, ICreditRequestRepository
{
    public CreditRequestRepository(ApplicationDbContext context) : base(context)
    {
    }
}