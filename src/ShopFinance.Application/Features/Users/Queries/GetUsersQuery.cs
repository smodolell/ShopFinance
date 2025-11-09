using ShopFinance.Application.Features.Users.DTOs;

namespace ShopFinance.Application.Features.Users.Queries;

public class GetUsersQuery : IQuery<PagedResult<List<UserListItemDto>>>
{
    private static readonly HashSet<string> _validSortColumns = new()
    {
        nameof(UserListItemDto.Id),
        nameof(UserListItemDto.UserName),
        nameof(UserListItemDto.FirstName),
        nameof(UserListItemDto.LastName),
    };

    private int _page = 1;
    private int _pageSize = 10;
    private string _sortColumn = nameof(UserListItemDto.UserName);

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
        set => _sortColumn = _validSortColumns.Contains(value) ? value : nameof(UserListItemDto.UserName);
    }

    public bool SortDescending { get; set; }

    public string? SearchText { get; set; }
}
