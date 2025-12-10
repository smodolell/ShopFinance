using ShopFinance.Application.Features.QuotationPlans.DTOs;

namespace ShopFinance.Application.Features.QuotationPlans.Queries;

public class GetQuotationPlanByIdQuery : IQuery<Result<QuotationPlanDetailsDto>>
{
    public int Id { get; set; }
}
