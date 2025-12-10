using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Repositories;

namespace ShopFinance.Infrastructure.Data.Repositories;

public class QuotationPlanPaymentTermRepository : Repository<QuotationPlanPaymentTerm, Guid>, IQuotationPlanPaymentTermRepository
{
    public QuotationPlanPaymentTermRepository(ApplicationDbContext context) : base(context)
    {
    }
}
