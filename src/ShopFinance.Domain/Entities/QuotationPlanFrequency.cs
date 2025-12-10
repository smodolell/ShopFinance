namespace ShopFinance.Domain.Entities;

public class QuotationPlanFrequency : BaseEntity<Guid>
{
 
    public int QuotationPlanId { get; set; }
    public int FrequencyId { get; set; }

    public bool IsDefault { get; set; }
    public int Order { get; set; }

    public QuotationPlan QuotationPlan { get; set; } = null!;
    public Frequency Frequency { get; set; } = null!;
}

