using ShopFinance.Application.Features.StockMovements.DTOs;
using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Specifications;

namespace ShopFinance.Application.Features.StockMovements.Queries;

internal class GetStockMovementsQueryHandler : IQueryHandler<GetStockMovementsQuery, PagedResult<List<StockMovementListItemDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDynamicSorter _sorter;
    private readonly IPaginator _paginator;

    public GetStockMovementsQueryHandler(IUnitOfWork unitOfWork, IDynamicSorter sorter, IPaginator paginator)
    {
        _unitOfWork = unitOfWork;
        _sorter = sorter;
        _paginator = paginator;
    }

    public async Task<PagedResult<List<StockMovementListItemDto>>> HandleAsync(GetStockMovementsQuery message, CancellationToken cancellationToken = default)
    {
        var spec = new StockMovementSpec(
            message.WarehouseId,
            message.ProductId,
            message.MovementType,
            message.Source,
            message.MovementDateFrom,
            message.MovementDateTo,
            message.SearchText
        );

        var query = _unitOfWork.StockMovements
            .ApplySpecification(spec);
           
        query = _sorter.ApplySort(query, message.SortColumn, message.SortDescending);

        return await _paginator.PaginateAsync<StockMovement, StockMovementListItemDto>(
            query,
            message.Page,
            message.PageSize,
            cancellationToken
        );
    }
}