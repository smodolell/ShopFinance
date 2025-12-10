namespace ShopFinance.Application.Features.Phases.DTOs;

public class PhaseStateDto
{
    public int Id { get; set; }
    public string PhaseStateName { get; set; } = string.Empty;
    public bool Initial { get; set; }
    public bool Edition { get; set; }
    public bool Completed { get; set; }
    public bool Canceled { get; set; }
    public bool Refused { get; set; }
    public bool PreviousPhase { get; set; }
}
