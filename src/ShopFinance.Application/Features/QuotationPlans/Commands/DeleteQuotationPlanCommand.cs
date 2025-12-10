namespace ShopFinance.Application.Features.QuotationPlans.Commands;

public class DeleteQuotationPlanCommand : ICommand<Result>
{
    public int QuotationPlanId { get; set; }
}
