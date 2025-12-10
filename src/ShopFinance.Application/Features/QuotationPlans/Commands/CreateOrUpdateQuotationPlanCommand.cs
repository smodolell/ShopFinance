namespace ShopFinance.Application.Features.QuotationPlans.Commands;

public class CreateOrUpdateQuotationPlanCommand : ICommand<Result<int>>
{
    public int QuotationPlanId { get; set; }
    public string PlanName { get; set; } = string.Empty;
    public DateTime? InitialEffectiveDate { get; set; }
    public DateTime? FinalEffectiveDate { get; set; }
    public int? PhaseIdInitial { get; set; }
    public int? PhaseIdFinal { get; set; }
    public bool Active { get; set; }
    public List<QuotationPlanPhaseCommand> Phases { get; set; } = new();
}

public class QuotationPlanPhaseCommand
{
    public int PhaseId { get; set; }
    public string PhaseName { get; set; } = string.Empty;

    public bool IsInitial { get; set; }
    public bool IsFinal { get; set; }
    public bool Required { get; set; }
    public decimal Order { get; set; }
    public bool Active { get; set; } = true;
}
