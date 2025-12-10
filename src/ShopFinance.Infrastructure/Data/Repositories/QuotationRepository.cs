using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Repositories;

namespace ShopFinance.Infrastructure.Data.Repositories;

public class QuotationRepository : Repository<Quotation, Guid>, IQuotationRepository
{
    public QuotationRepository(ApplicationDbContext context) : base(context)
    {
    }
}
