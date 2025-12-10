namespace ShopFinance.Application.Features.QuotationPlans.DTOs;

public class QuotationPlanDetailsDto
{
    public int Id { get; set; }
    public string PlanName { get; set; } = string.Empty;
    public DateTime InitialEffectiveDate { get; set; }
    public DateTime FinalEffectiveDate { get; set; }
    public List<QuotationPlanPhaseDto> Phases { get; set; } = new();
    public bool IsActive => DateTime.UtcNow >= InitialEffectiveDate && DateTime.UtcNow <= FinalEffectiveDate;
}
