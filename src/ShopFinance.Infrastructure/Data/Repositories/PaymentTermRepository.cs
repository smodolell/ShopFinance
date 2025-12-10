using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Repositories;

namespace ShopFinance.Infrastructure.Data.Repositories;

public class PaymentTermRepository : Repository<PaymentTerm, int>, IPaymentTermRepository
{
    public PaymentTermRepository(ApplicationDbContext context) : base(context)
    {
    }
}