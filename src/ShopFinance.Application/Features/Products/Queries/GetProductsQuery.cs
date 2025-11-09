using ShopFinance.Application.Features.Products.DTOs;
using ShopFinance.Domain.Enums;

namespace ShopFinance.Application.Features.Products.Queries;

public class GetProductsQuery : IQuery<PagedResult<List<ProductListItemDto>>>
{
    private static readonly HashSet<string> _validSortColumns = new()
    {
        nameof(ProductListItemDto.Name),
        nameof(ProductListItemDto.Description),
    };

    private int _page = 1;
    private int _pageSize = 10;
    private string _sortColumn = nameof(ProductListItemDto.Description);

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
        set => _sortColumn = _validSortColumns.Contains(value) ? value : nameof(ProductListItemDto.Name);
    }

    public bool SortDescending { get; set; }


    public ProductState? State { get; set; }

    public string? SearchText { get; set; }

    public DateTime? CreatedFrom { get; set; }
    public DateTime? CreatedTo { get; set; }
}
