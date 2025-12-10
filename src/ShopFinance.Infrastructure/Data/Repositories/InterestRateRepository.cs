using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Repositories;

namespace ShopFinance.Infrastructure.Data.Repositories;

public class InterestRateRepository : Repository<InterestRate, int>, IInterestRateRepository
{
    public InterestRateRepository(ApplicationDbContext context) : base(context)
    {
    }
}