using ShopFinance.Application.Features.Roles.DTOs;

namespace ShopFinance.Application.Features.Roles.Queries;

public class GetRolesQuery : IQuery<PagedResult<List<RoleDto>>>
{
    private static readonly HashSet<string> _validSortColumns = new()
    {
        nameof(RoleDto.Id),
        nameof(RoleDto.Name),
        nameof(RoleDto.Description),
    };

    private int _page = 1;
    private int _pageSize = 10;
    private string _sortColumn = nameof(RoleDto.Name);

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
        set => _sortColumn = _validSortColumns.Contains(value) ? value : nameof(RoleDto.Name);
    }

    public bool SortDescending { get; set; }

    public string? SearchText { get; set; }
}
