using ShopFinance.Domain.Entities;

namespace ShopFinance.Domain.Repositories;

public interface IPaymentRepository : IRepository<Payment, Guid>
{
}

