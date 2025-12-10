using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Repositories;

namespace ShopFinance.Infrastructure.Data.Repositories;

public class QuotationPlanFrequencyRepository : Repository<QuotationPlanFrequency, Guid>, IQuotationPlanFrequencyRepository
{
    public QuotationPlanFrequencyRepository(ApplicationDbContext context) : base(context)
    {
    }
}
