using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Repositories;

namespace ShopFinance.Infrastructure.Data.Repositories;

public class PhaseRepository : Repository<Phase, int>, IPhaseRepository
{
    public PhaseRepository(ApplicationDbContext context) : base(context)
    {
    }
}
