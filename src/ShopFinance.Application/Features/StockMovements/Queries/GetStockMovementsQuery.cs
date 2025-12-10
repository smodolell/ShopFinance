using ShopFinance.Application.Features.StockMovements.DTOs;
using ShopFinance.Domain.Enums;

namespace ShopFinance.Application.Features.StockMovements.Queries;

public class GetStockMovementsQuery : IQuery<PagedResult<List<StockMovementListItemDto>>>
{
    private static readonly HashSet<string> _validSortColumns = new()
    {
        nameof(StockMovementListItemDto.MovementDate),
        nameof(StockMovementListItemDto.ProductName),
        nameof(StockMovementListItemDto.WarehouseName),
        nameof(StockMovementListItemDto.MovementType),
        nameof(StockMovementListItemDto.Quantity),
        nameof(StockMovementListItemDto.CreatedAt)
    };

    private int _page = 1;
    private int _pageSize = 10;
    private string _sortColumn = nameof(StockMovementListItemDto.MovementDate);

    public int Page
    {
        get => _page;
        set => _page = value < 1 ? 1 : value;
    }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value switch
        {
            < 1 => 10,
            > 100 => 100,
            _ => value
        };
    }

    public string SortColumn
    {
        get => _sortColumn;
        set => _sortColumn = _validSortColumns.Contains(value) ? value : nameof(StockMovementListItemDto.MovementDate);
    }

    public bool SortDescending { get; set; } = true; // Por defecto más recientes primero

    // Filtros
    public Guid? WarehouseId { get; set; }
    public Guid? ProductId { get; set; }
    public MovementType? MovementType { get; set; }
    public MovementSource? Source { get; set; }
    public DateTime? MovementDateFrom { get; set; }
    public DateTime? MovementDateTo { get; set; }
    public string? SearchText { get; set; }
}