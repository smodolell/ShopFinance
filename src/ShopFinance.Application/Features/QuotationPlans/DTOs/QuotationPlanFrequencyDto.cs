namespace ShopFinance.Application.Features.QuotationPlans.DTOs;

public class QuotationPlanFrequencyDto
{
    public int FrequencyId { get; set; }
    public bool IsDefault { get; set; }
    public int Order { get; set; }
    public bool Active { get; set; } = true;
}
