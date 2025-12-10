namespace ShopFinance.Application.Services.InterestRates.DTOs;

public class InterestRateFilterDto
{
    private static readonly HashSet<string> _validSortColumns = new()
    {
        nameof(InterestRateListItemDto.RateName),
    };

    private int _page = 1;
    private int _pageSize = 10;
    private string _sortColumn = nameof(InterestRateListItemDto.RateName);

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
        set => _sortColumn = _validSortColumns.Contains(value) ? value : nameof(InterestRateListItemDto.RateName);
    }

    public bool SortDescending { get; set; }

    public string? SearchText { get; set; }
    public bool? IsActive { get; set; }
}