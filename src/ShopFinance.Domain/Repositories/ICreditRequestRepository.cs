using ShopFinance.Domain.Entities;
namespace ShopFinance.Domain.Repositories;

public interface ICreditRequestRepository : IRepository<CreditRequest, Guid>
{
}

public interface IQuotationPlanFrequencyRepository : IRepository<QuotationPlanFrequency, Guid>
{
}
