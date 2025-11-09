using ShopFinance.Application.Features.Customers.DTOs;
using ShopFinance.Application.Features.Products.DTOs;
using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Specifications;

namespace ShopFinance.Application.Features.Customers.Queries;

internal class GetCustomersQueryHandler : IQueryHandler<GetCustomersQuery, PagedResult<List<CustomerListItemDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDynamicSorter _sorter;
    private readonly IPaginator _paginator;

    public GetCustomersQueryHandler(IUnitOfWork unitOfWork, IDynamicSorter sorter, IPaginator paginator)
    {
        _unitOfWork = unitOfWork;
        _sorter = sorter;
        _paginator = paginator;
    }
    public async Task<PagedResult<List<CustomerListItemDto>>> HandleAsync(GetCustomersQuery message, CancellationToken cancellationToken = default)
    {
        var spec = new CustomerSpec(message.SearchText);

        var query = _unitOfWork.Customers.ApplySpecification(spec);

        query = _sorter.ApplySort(query, message.SortColumn, message.SortDescending);

        return await _paginator.PaginateAsync<Customer, CustomerListItemDto>(
            query,
            message.Page,
            message.PageSize,
            cancellationToken
        );
    }
}