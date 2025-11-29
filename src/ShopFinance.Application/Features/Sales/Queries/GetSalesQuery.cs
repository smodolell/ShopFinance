using ShopFinance.Application.Features.Sales.DTOs;
using ShopFinance.Domain.Enums;

namespace ShopFinance.Application.Features.Sales.Queries;

public class GetSalesQuery : IQuery<PagedResult<List<SaleListItemDto>>>
{
    private static readonly HashSet<string> _validSortColumns = new()
    {
        nameof(SaleListItemDto.SaleNumber),
        nameof(SaleListItemDto.SaleDate),
        nameof(SaleListItemDto.TotalAmount),
        nameof(SaleListItemDto.CustomerName),
        nameof(SaleListItemDto.Status),
        nameof(SaleListItemDto.PaymentMethod)
    };

    private int _page = 1;
    private int _pageSize = 10;
    private string _sortColumn = nameof(SaleListItemDto.SaleDate);

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
        set => _sortColumn = _validSortColumns.Contains(value) ? value : nameof(SaleListItemDto.SaleDate);
    }

    public bool SortDescending { get; set; }

    // Filtros específicos para ventas
    public SaleStatus? Status { get; set; }
    public PaymentMethod? PaymentMethod { get; set; }
    public Guid? CustomerId { get; set; }
    public string? SearchText { get; set; }
    public DateTime? SaleDateFrom { get; set; }
    public DateTime? SaleDateTo { get; set; }
    public DateTime? CreatedFrom { get; set; }
    public DateTime? CreatedTo { get; set; }
}
