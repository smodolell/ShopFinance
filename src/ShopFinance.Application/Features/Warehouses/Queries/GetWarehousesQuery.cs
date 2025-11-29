using ShopFinance.Application.Features.Warehouses.DTOs;
using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Specifications;

namespace ShopFinance.Application.Features.Warehouses.Queries;

public class GetWarehousesQuery :IQuery<PagedResult<List<WarehouseListItemDto>>>
{
    private static readonly HashSet<string> _validSortColumns = new()
    {
        nameof(WarehouseListItemDto.Code),
        nameof(WarehouseListItemDto.Name),
        nameof(WarehouseListItemDto.Type),
        nameof(WarehouseListItemDto.CreatedAt)
    };

    private int _page = 1;
    private int _pageSize = 10;
    private string _sortColumn = nameof(WarehouseListItemDto.CreatedAt);

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
        set => _sortColumn = _validSortColumns.Contains(value) ? value : nameof(WarehouseListItemDto.CreatedAt);
    }

    public bool SortDescending { get; set; }

    public string? SearchText { get; set; }

    public DateTime? CreatedFrom { get; set; }
    public DateTime? CreatedTo { get; set; }
}
public class GetWarehousesQueryHandler : IQueryHandler<GetWarehousesQuery, PagedResult<List<WarehouseListItemDto>>>{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPaginator _paginator;
    private readonly IDynamicSorter _sorter;

    public GetWarehousesQueryHandler(IUnitOfWork unitOfWork, IPaginator paginator, IDynamicSorter sorter)
    {
        _unitOfWork = unitOfWork;
        _paginator = paginator;
        _sorter = sorter;
    }
    public async Task<PagedResult<List<WarehouseListItemDto>>> HandleAsync(GetWarehousesQuery message, CancellationToken cancellationToken = default)
    {
        var spec = new WarehouseSpec(message.SearchText, message.CreatedFrom, message.CreatedTo);

        var query = _unitOfWork.Warehouses.ApplySpecification(spec);

        query = _sorter.ApplySort(query, message.SortColumn, message.SortDescending);

        return await _paginator.PaginateAsync<Warehouse, WarehouseListItemDto>(
            query,
            message.Page,
            message.PageSize,
            cancellationToken
        );
    }
}
