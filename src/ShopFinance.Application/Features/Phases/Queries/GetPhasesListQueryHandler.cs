using Microsoft.EntityFrameworkCore;
using ShopFinance.Application.Features.Phases.DTOs;
using ShopFinance.Domain.Specifications;

namespace ShopFinance.Application.Features.Phases.Queries;

internal class GetPhasesListQueryHandler : IQueryHandler<GetPhasesListQuery, List<PhaseListItemDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPhasesListQueryHandler(IUnitOfWork unitOfWork)
    {
        this._unitOfWork = unitOfWork;
    }
    public async Task<List<PhaseListItemDto>> HandleAsync(GetPhasesListQuery message, CancellationToken cancellationToken = default)
    {
        var spec = new PhaseSpec(null, message.IsInitial, message.IsFinal, message.Required);
        var phases = _unitOfWork.Phases.ApplySpecification(spec);

        return await phases.Select(p => new PhaseListItemDto
        {
            Id = p.Id,
            Code = p.Code,
            PhaseName = p.PhaseName,
            Route = p.Route,
            IsInitial = p.IsInitial,
            IsFinal = p.IsFinal,
            Required = p.Required,
            Order = p.Order,
            StateCount = p.States.Count,
        }).ToListAsync();
    }
}
