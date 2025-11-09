using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Repositories;

namespace ShopFinance.Infrastructure.Data.Repositories;

public class FrequencyRepository : Repository<Frequency, int>, IFrequencyRepository
{
    public FrequencyRepository(ApplicationDbContext context) : base(context)
    {
    }
}