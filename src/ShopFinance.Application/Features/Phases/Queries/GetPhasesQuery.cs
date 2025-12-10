using ShopFinance.Application.Features.Phases.DTOs;

namespace ShopFinance.Application.Features.Phases.Queries;

public class GetPhasesQuery : IQuery<PagedResult<List<PhaseListItemDto>>>
{
    private static readonly HashSet<string> _validSortColumns = new()
    {
        nameof(PhaseListItemDto.Code),
        nameof(PhaseListItemDto.PhaseName),
        nameof(PhaseListItemDto.Order),
        nameof(PhaseListItemDto.IsInitial),
        nameof(PhaseListItemDto.IsFinal),
        nameof(PhaseListItemDto.Required)
    };

    private int _page = 1;
    private int _pageSize = 10;
    private string _sortColumn = nameof(PhaseListItemDto.Order);

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
        set => _sortColumn = _validSortColumns.Contains(value) ? value : nameof(PhaseListItemDto.Order);
    }

    public bool SortDescending { get; set; }

    public string? SearchText { get; set; }

    public bool? IsInitial { get; set; }

    public bool? IsFinal { get; set; }

    public bool? Required { get; set; }
}
