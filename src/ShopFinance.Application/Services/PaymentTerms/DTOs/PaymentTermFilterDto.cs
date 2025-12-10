// PaymentTermEditDto.cs
namespace ShopFinance.Application.Services.PaymentTerms.DTOs;

public class PaymentTermFilterDto
{
    private static readonly HashSet<string> _validSortColumns = new()
    {
        nameof(PaymentTermListItemDto.Name),
        nameof(PaymentTermListItemDto.Code),
        nameof(PaymentTermListItemDto.NumberOfPayments),
        nameof(PaymentTermListItemDto.ApproximateDays)
    };

    private int _page = 1;
    private int _pageSize = 10;
    private string _sortColumn = nameof(PaymentTermListItemDto.Name);

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
        set => _sortColumn = _validSortColumns.Contains(value) ? value : nameof(PaymentTermListItemDto.Name);
    }

    public bool SortDescending { get; set; }
    public string? SearchText { get; set; }
    public bool? IsActive { get; set; }
    public int? MinMonths { get; set; }
    public int? MaxMonths { get; set; }
}
