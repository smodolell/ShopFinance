namespace ShopFinance.Application.Features.Phases.DTOs;

public class PhaseDetailsDto : PhaseDto
{
    public List<PhaseStateDto> States { get; set; } = new();
}
