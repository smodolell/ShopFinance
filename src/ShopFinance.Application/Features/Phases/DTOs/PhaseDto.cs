namespace ShopFinance.Application.Features.Phases.DTOs;

public class PhaseDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string PhaseName { get; set; } = string.Empty;
    public string Route { get; set; } = string.Empty;
    public bool IsInitial { get; set; }
    public bool IsFinal { get; set; }
    public bool Required { get; set; }
    public decimal Order { get; set; }
    public int StateCount { get; set; }
}
