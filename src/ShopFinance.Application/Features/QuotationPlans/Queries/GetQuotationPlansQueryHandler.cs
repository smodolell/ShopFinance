using ShopFinance.Application.Features.QuotationPlans.DTOs;
using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Specifications;

namespace ShopFinance.Application.Features.QuotationPlans.Queries;

internal class GetQuotationPlansQueryHandler : IQueryHandler<GetQuotationPlansQuery, PagedResult<List<QuotationPlanListItemDto>>>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IDynamicSorter _sorter;
    private readonly IPaginator _paginator;

    public GetQuotationPlansQueryHandler(IUnitOfWork unitOfWork, IDynamicSorter sorter, IPaginator paginator)
    {
        _unitOfWork = unitOfWork;
        _sorter = sorter;
        _paginator = paginator;
    }

    public async Task<PagedResult<List<QuotationPlanListItemDto>>> HandleAsync(
        GetQuotationPlansQuery message,
        CancellationToken cancellationToken = default)
    {
        var spec = new QuotationPlanSpec(
            searchText: message.SearchText,
            activeOnly: message.ActiveOnly,
            effectiveDate: message.EffectiveDate,
            includePhases: true);


        var query = _unitOfWork.QuotationPlans.ApplySpecification(spec);

        query = _sorter.ApplySort(query, message.SortColumn, message.SortDescending);

        return await _paginator.PaginateAsync<QuotationPlan, QuotationPlanListItemDto>(
            query,
            message.Page,
            message.PageSize,
            cancellationToken
        );
    }
}