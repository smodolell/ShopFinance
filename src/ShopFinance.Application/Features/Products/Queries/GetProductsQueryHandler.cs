using ShopFinance.Application.Features.Products.DTOs;
using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Specifications;

namespace ShopFinance.Application.Features.Products.Queries;

internal class GetProductsQueryHandler : IQueryHandler<GetProductsQuery, PagedResult<List<ProductListItemDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDynamicSorter _sorter;
    private readonly IPaginator _paginator;

    public GetProductsQueryHandler(IUnitOfWork unitOfWork, IDynamicSorter sorter, IPaginator paginator)
    {
        _unitOfWork = unitOfWork;
        _sorter = sorter;
        _paginator = paginator;
    }
    public async Task<PagedResult<List<ProductListItemDto>>> HandleAsync(GetProductsQuery message, CancellationToken cancellationToken = default)
    {
        var spec = new ProductSpec { SearchText = message.SearchText };

        var query = _unitOfWork.Products.ApplySpecification(spec);

        query = _sorter.ApplySort(query, message.SortColumn, message.SortDescending);

        return await _paginator.PaginateAsync<Product, ProductListItemDto>(
            query,
            message.Page,
            message.PageSize,
            cancellationToken
        );
    }
}