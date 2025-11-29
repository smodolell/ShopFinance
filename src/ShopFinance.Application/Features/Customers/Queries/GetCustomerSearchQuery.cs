using ShopFinance.Application.Features.Customers.DTOs;
using ShopFinance.Domain.Common.Interfaces;
using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Specifications;

namespace ShopFinance.Application.Features.Customers.Queries;

public class GetCustomerSearchQuery : IQuery<PagedResult<List<CustomerSearchDto>>>
{
    public string? SearchText { get; set; }
}
internal class GetCustomerSearchQueryHandler : IQueryHandler<GetCustomerSearchQuery, PagedResult<List<CustomerSearchDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDynamicSorter _sorter;
    private readonly IPaginator _paginator;

    public GetCustomerSearchQueryHandler(IUnitOfWork unitOfWork, IDynamicSorter sorter, IPaginator paginator)
    {
        _unitOfWork = unitOfWork;
        _sorter = sorter;
        _paginator = paginator;
    }
  

    public async Task<PagedResult<List<CustomerSearchDto>>> HandleAsync(GetCustomerSearchQuery message, CancellationToken cancellationToken = default)
    {
        var spec = new CustomerSpec(message.SearchText);
        var query = _unitOfWork.Customers.ApplySpecification(spec);

        return await _paginator.PaginateAsync<Customer, CustomerSearchDto>(
             query,
             1,
             10,
             cancellationToken
         );
    }
}