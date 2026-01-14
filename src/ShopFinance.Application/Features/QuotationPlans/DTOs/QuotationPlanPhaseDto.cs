namespace ShopFinance.Application.Features.QuotationPlans.DTOs;

public class QuotationPlanPhaseDto
{
    public Guid Id { get; set; }
    public int PhaseId { get; set; }
    public string PhaseName { get; set; } = string.Empty;
    public bool Active { get; set; }
    public bool Required { get; set; }
    public decimal Order { get; set; }
    public bool IsInitial { get; set; }
    public bool IsFinal { get; set; }
}