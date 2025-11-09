using ShopFinance.Application.Features.Customers.DTOs;

namespace ShopFinance.Application.Features.Customers.Queries;

public class GetCustomersQuery : IQuery<PagedResult<List<CustomerListItemDto>>>
{

    private static readonly HashSet<string> _validSortColumns = new()
    {
        nameof(CustomerListItemDto.Identifier),
        nameof(CustomerListItemDto.FirstName),
        nameof(CustomerListItemDto.LastName),
        nameof(CustomerListItemDto.Birthdate),
    };

    private int _page = 1;
    private int _pageSize = 10;
    private string _sortColumn = nameof(CustomerListItemDto.Birthdate);

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
        set => _sortColumn = _validSortColumns.Contains(value) ? value : nameof(CustomerListItemDto.LastName);
    }

    public bool SortDescending { get; set; }



    public string? SearchText { get; set; }

    public DateTime? CreatedFrom { get; set; }
    public DateTime? CreatedTo { get; set; }
}
