using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Repositories;

namespace ShopFinance.Infrastructure.Data.Repositories;

public class QuotationPlanRepository : Repository<QuotationPlan, int>, IQuotationPlanRepository
{
    public QuotationPlanRepository(ApplicationDbContext context) : base(context)
    {
    }
}
