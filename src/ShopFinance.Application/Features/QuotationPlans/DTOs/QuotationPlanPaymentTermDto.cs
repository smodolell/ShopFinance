namespace ShopFinance.Application.Features.QuotationPlans.DTOs;

public class QuotationPlanPaymentTermDto
{
    public int PaymentTermId { get; set; }
    public int? InterestRateId { get; set; }
    public decimal? SpecialRateOverride { get; set; }
    public bool IsPromotional { get; set; }
    public DateTime? PromotionEndDate { get; set; }
    public int Order { get; set; }
    public bool Active { get; set; } = true;
}