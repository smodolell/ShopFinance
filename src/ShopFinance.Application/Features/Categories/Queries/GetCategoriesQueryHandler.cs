using ShopFinance.Application.Features.Categories.DTOs;
using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Specifications;

namespace ShopFinance.Application.Features.Categories.Queries;

internal class GetCategoriesQueryHandler : IQueryHandler<GetCategoriesQuery, PagedResult<List<CategoryListItemDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPaginator _paginator;
    private readonly IDynamicSorter _sorter;

    public GetCategoriesQueryHandler(IUnitOfWork unitOfWork, IPaginator paginator, IDynamicSorter sorter)
    {
        _unitOfWork = unitOfWork;
        _paginator = paginator;
        _sorter = sorter;
    }
    public async Task<PagedResult<List<CategoryListItemDto>>> HandleAsync(GetCategoriesQuery message, CancellationToken cancellationToken = default)
    {
        var spec = new CategorySpec(message.SearchText,message.CreatedFrom,message.CreatedTo);

        var query = _unitOfWork.Categories.ApplySpecification(spec);

        query = _sorter.ApplySort(query, message.SortColumn, message.SortDescending);

        return await _paginator.PaginateAsync<Category, CategoryListItemDto>(
            query, 
            message.Page,
            message.PageSize,
            cancellationToken
        );
    }
}
