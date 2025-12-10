using ShopFinance.Application.Features.Phases.DTOs;

namespace ShopFinance.Application.Features.Phases.Queries;

public class GetPhasesListQuery : IQuery<List<PhaseListItemDto>>
{
    public bool? IsInitial { get; set; }

    public bool? IsFinal { get; set; }

    public bool? Required { get; set; }
}
