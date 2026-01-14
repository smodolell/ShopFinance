using ShopFinance.Application.Features.QuotationPlans.DTOs;

namespace ShopFinance.Application.Features.QuotationPlans.Commands;

public class CreateQuotationPlanCommand : ICommand<Result<int>>
{
    public QuotationPlanEditDto Model { get; set; } = new();
}
