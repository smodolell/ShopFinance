namespace ShopFinance.Domain.Entities;

public class QuotationPlanPhase : BaseEntity<Guid>
{
    public int QuotationPlanId { get; set; }
    public int PhaseId { get; set; }
    public bool Active { get; set; }
    public QuotationPlan QuotationPlan { get; set; } = null!;
    public Phase Phase { get; set; } = null!;
}
