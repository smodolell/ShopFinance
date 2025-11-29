using ShopFinance.Application.Features.Sales.DTOs;
using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Specifications;

namespace ShopFinance.Application.Features.Sales.Queries;

internal class GetSalesQueryHandler : IQueryHandler<GetSalesQuery, PagedResult<List<SaleListItemDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDynamicSorter _sorter;
    private readonly IPaginator _paginator;
    private readonly ILogger<GetSalesQueryHandler> _logger;

    public GetSalesQueryHandler(
        IUnitOfWork unitOfWork,
        IDynamicSorter sorter,
        IPaginator paginator,
        ILogger<GetSalesQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _sorter = sorter;
        _paginator = paginator;
        _logger = logger;
    }

    public async Task<PagedResult<List<SaleListItemDto>>> HandleAsync(
        GetSalesQuery message,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var spec = new SaleSpec(
                message.SearchText,
                message.Status,
                message.PaymentMethod,
                message.CustomerId,
                message.SaleDateFrom,
                message.SaleDateTo
            );

            var query = _unitOfWork.Sales.ApplySpecification(spec);

            query = _sorter.ApplySort(query, message.SortColumn, message.SortDescending);

            return await _paginator.PaginateAsync<Sale, SaleListItemDto>(
                query,
                message.Page,
                message.PageSize,
                cancellationToken
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener la lista de ventas. Filtros: {@Filters}", message);
            throw;
        }
    }
}