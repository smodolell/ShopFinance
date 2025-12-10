using ShopFinance.Application.Features.Phases.DTOs;
using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Specifications;

namespace ShopFinance.Application.Features.Phases.Queries;

internal class GetPhasesQueryHandler : IQueryHandler<GetPhasesQuery, PagedResult<List<PhaseListItemDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPaginator _paginator;
    private readonly IDynamicSorter _sorter;
    private readonly ILogger<GetPhasesQueryHandler> _logger;

    public GetPhasesQueryHandler(
        IUnitOfWork unitOfWork,
        IPaginator paginator,
        IDynamicSorter sorter,
        ILogger<GetPhasesQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _paginator = paginator;
        _sorter = sorter;
        _logger = logger;
    }

    public async Task<PagedResult<List<PhaseListItemDto>>> HandleAsync(
        GetPhasesQuery query,
        CancellationToken cancellationToken = default)
    {
        var spec = new PhaseSpec(query.SearchText, query.IsInitial, query.IsFinal, query.Required);
        var dbQuery = _unitOfWork.Phases.ApplySpecification(spec);

        // Aplicar ordenamiento
        dbQuery = _sorter.ApplySort(dbQuery, query.SortColumn, query.SortDescending);

        // Paginar y mapear
        var result = await _paginator.PaginateAsync<Phase, PhaseListItemDto>(
            dbQuery,
            query.Page,
            query.PageSize,
            cancellationToken);

        // Enriquecer con información de uso
        foreach (var phase in result.Value)
        {
            //phase.IsInUse = await _unitOfWork.Phases.IsPhaseInUseAsync(phase.Id, cancellationToken);
            phase.StateCount = (await _unitOfWork.Phases.GetByIdAsync(phase.Id, cancellationToken))
                ?.States.Count ?? 0;
        }

        _logger.LogInformation("Obtenidas {Count} fases, página {Page}",
            result.Value.Count, query.Page);

        return result;

    }
}