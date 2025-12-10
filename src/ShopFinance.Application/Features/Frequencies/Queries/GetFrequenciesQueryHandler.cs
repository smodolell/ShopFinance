using ShopFinance.Application.Features.Frequencies.DTOs;
using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Specifications;

namespace ShopFinance.Application.Features.Frequencies.Queries;

internal class GetFrequenciesQueryHandler : IQueryHandler<GetFrequenciesQuery, PagedResult<List<FrequencyListItemDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPaginator _paginator;
    private readonly IDynamicSorter _sorter;

    public GetFrequenciesQueryHandler(IUnitOfWork unitOfWork, IPaginator paginator, IDynamicSorter sorter)
    {
        _unitOfWork = unitOfWork;
        _paginator = paginator;
        _sorter = sorter;
    }
    public async Task<PagedResult<List<FrequencyListItemDto>>> HandleAsync(GetFrequenciesQuery message, CancellationToken cancellationToken = default)
    {
        var spec = new FrequencySpec(message.SearchText,true);

        var query = _unitOfWork.Frequencies.ApplySpecification(spec);

        query = _sorter.ApplySort(query, message.SortColumn, message.SortDescending);

        return await _paginator.PaginateAsync<Frequency, FrequencyListItemDto>(
            query,
            message.Page,
            message.PageSize,
            cancellationToken
        );
    }
}
