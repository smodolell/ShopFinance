namespace ShopFinance.Domain.Entities;

public class QuotationPlan : BaseEntity<int>
{
    public int TaxRateId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string PlanName { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    public DateTime InitialEffectiveDate { get; set; }
    public DateTime FinalEffectiveDate { get; set; }
    public bool IsActive { get; set; } = true;

    public TaxRate TaxRate { get; set; } = null!;

    public ICollection<QuotationPlanPhase> Phases { get; set; } = new HashSet<QuotationPlanPhase>();
    public ICollection<QuotationPlanFrequency> Frequencies { get; set; } = new HashSet<QuotationPlanFrequency>();
    public ICollection<QuotationPlanPaymentTerm> PaymentTerms { get; set; } = new HashSet<QuotationPlanPaymentTerm>();
}
