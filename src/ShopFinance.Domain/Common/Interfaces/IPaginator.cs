using Ardalis.Result;

namespace ShopFinance.Domain.Common.Interfaces;

public interface IPaginator
{
    Task<PagedResult<List<TDestination>>> PaginateAsync<T, TDestination>(
        IQueryable<T> query,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
        where T : class
        where TDestination : class;
}
