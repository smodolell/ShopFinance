using ShopFinance.Application.Features.Users.DTOs;
using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Specifications;

namespace ShopFinance.Application.Features.Users.Queries;

internal class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, PagedResult<List<UserListItemDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDynamicSorter _sorter;
    private readonly IPaginator _paginator;

    public GetUsersQueryHandler(IUnitOfWork unitOfWork, IDynamicSorter sorter, IPaginator paginator)
    {
        _unitOfWork = unitOfWork;
        _sorter = sorter;
        _paginator = paginator;
    }

    public async Task<PagedResult<List<UserListItemDto>>> HandleAsync(GetUsersQuery message, CancellationToken cancellationToken = default)
    {

        var spec = new UserSpec(message.SearchText);

        var query = _unitOfWork.Users.ApplySpecification(spec);

        query = _sorter.ApplySort(query, message.SortColumn, message.SortDescending);

        return await _paginator.PaginateAsync<User, UserListItemDto>(
            query,
            message.Page,
            message.PageSize,
            cancellationToken
        );
    }
}
