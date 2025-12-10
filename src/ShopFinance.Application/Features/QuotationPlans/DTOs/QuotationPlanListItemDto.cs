namespace ShopFinance.Application.Features.QuotationPlans.DTOs;

public class QuotationPlanListItemDto
{
    public int Id { get; set; }
    public string PlanName { get; set; } = string.Empty;
    public DateTime InitialEffectiveDate { get; set; }
    public DateTime FinalEffectiveDate { get; set; }
    public bool IsActive => DateTime.UtcNow >= InitialEffectiveDate && DateTime.UtcNow <= FinalEffectiveDate;
    public int PhaseCount { get; set; }
}
