using ShopFinance.Application.Features.Roles.DTOs;
using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Specifications;

namespace ShopFinance.Application.Features.Roles.Queries;

internal class GetRolesQueryHandler : IQueryHandler<GetRolesQuery, PagedResult<List<RoleDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDynamicSorter _sorter;
    private readonly IPaginator _paginator;

    public GetRolesQueryHandler(IUnitOfWork unitOfWork, IDynamicSorter sorter, IPaginator paginator)
    {
        _unitOfWork = unitOfWork;
        _sorter = sorter;
        _paginator = paginator;
    }
    public async Task<PagedResult<List<RoleDto>>> HandleAsync(GetRolesQuery message, CancellationToken cancellationToken = default)
    {

        var spec = new RoleSpec(message.SearchText);

        var query = _unitOfWork.Roles.ApplySpecification(spec);

        query = _sorter.ApplySort(query, message.SortColumn, message.SortDescending);

        return await _paginator.PaginateAsync<Role, RoleDto>(
            query,
            message.Page,
            message.PageSize,
            cancellationToken
        );

    }
}
