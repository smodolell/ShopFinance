using ShopFinance.Application.Features.Categories.DTOs;

namespace ShopFinance.Application.Features.Categories.Queries;

public class GetCategoriesQuery : IQuery<PagedResult<List<CategoryDto>>>
{
    private static readonly HashSet<string> _validSortColumns = new()
    {
        nameof(CategoryDto.Code),
        nameof(CategoryDto.Name),
    };

    private int _page = 1;
    private int _pageSize = 10;
    private string _sortColumn = nameof(CategoryDto.Code);

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
        set => _sortColumn = _validSortColumns.Contains(value) ? value : nameof(CategoryDto.Code);
    }

    public bool SortDescending { get; set; }

    public string? SearchText { get; set; }
}
