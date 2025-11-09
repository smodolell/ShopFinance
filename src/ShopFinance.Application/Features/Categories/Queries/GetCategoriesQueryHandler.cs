using ShopFinance.Application.Features.Categories.DTOs;
using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Specifications;

namespace ShopFinance.Application.Features.Categories.Queries;

internal class GetCategoriesQueryHandler : IQueryHandler<GetCategoriesQuery, PagedResult<List<CategoryDto>>>
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
    public async Task<PagedResult<List<CategoryDto>>> HandleAsync(GetCategoriesQuery message, CancellationToken cancellationToken = default)
    {

        var query = _unitOfWork.Categories.ApplySpecification(new CategorySpec { SearchText = message.SearchText });

        query = _sorter.ApplySort(query, message.SortColumn, message.SortDescending);

        return await _paginator.PaginateAsync<Category, CategoryDto>(
            query, 
            message.Page,
            message.PageSize,
            cancellationToken
        );
    }
}
