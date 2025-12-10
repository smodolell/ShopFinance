public class ToggleQuotationPlanStatusCommand : ICommand<Result>
{
    public int QuotationPlanId { get; set; }
    public bool IsActive { get; set; }
}
