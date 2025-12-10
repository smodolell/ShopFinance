using ShopFinance.Application.Features.Frequencies.DTOs;
using ShopFinance.Domain.Entities;

namespace ShopFinance.Application.Features.Frequencies.Queries;

public class GetFrequenciesQuery : IQuery<PagedResult<List<FrequencyListItemDto>>>
{
    private static readonly HashSet<string> _validSortColumns = new()
    {
        nameof(FrequencyListItemDto.Code),
        nameof(FrequencyListItemDto.Name),
    };

    private int _page = 1;
    private int _pageSize = 10;
    private string _sortColumn = nameof(Frequency.Code);

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
        set => _sortColumn = _validSortColumns.Contains(value) ? value : nameof(Frequency.Code);
    }

    public bool SortDescending { get; set; }

    public string? SearchText { get; set; }

}
