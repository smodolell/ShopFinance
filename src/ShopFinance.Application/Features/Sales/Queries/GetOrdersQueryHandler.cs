using ShopFinance.Application.Features.Sales.DTOs;
using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Specifications;

namespace ShopFinance.Application.Features.Sales.Queries;

internal class GetOrdersQueryHandler : IQueryHandler<GetOrdersQuery, PagedResult<List<OrderListItemDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDynamicSorter _sorter;
    private readonly IPaginator _paginator;
    private readonly ILogger<GetOrdersQueryHandler> _logger;

    public GetOrdersQueryHandler(
        IUnitOfWork unitOfWork,
        IDynamicSorter sorter,
        IPaginator paginator,
        ILogger<GetOrdersQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _sorter = sorter;
        _paginator = paginator;
        _logger = logger;
    }

    public async Task<PagedResult<List<OrderListItemDto>>> HandleAsync(
        GetOrdersQuery message,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var spec = new OrderSpec(
                message.SearchText,
                message.Status,
                message.CustomerId,
                message.OrderDateFrom,
                message.OrderDateTo,
                message.RequiredDateFrom,
                message.RequiredDateTo
            );

            var query = _unitOfWork.Orders.ApplySpecification(spec);

            query = _sorter.ApplySort(query, message.SortColumn, message.SortDescending);

            return await _paginator.PaginateAsync<Order, OrderListItemDto>(
                query,
                message.Page,
                message.PageSize,
                cancellationToken
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener la lista de órdenes. Filtros: {@Filters}", message);
            throw;
        }
    }
}