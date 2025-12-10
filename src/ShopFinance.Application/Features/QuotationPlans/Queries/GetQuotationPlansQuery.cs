using ShopFinance.Application.Features.QuotationPlans.DTOs;

namespace ShopFinance.Application.Features.QuotationPlans.Queries;

public class GetQuotationPlansQuery : IQuery<PagedResult<List<QuotationPlanListItemDto>>>
{
    private static readonly HashSet<string> _validSortColumns = new()
    {
        nameof(QuotationPlanListItemDto.PlanName),
        nameof(QuotationPlanListItemDto.InitialEffectiveDate),
        nameof(QuotationPlanListItemDto.FinalEffectiveDate),
    };

    private int _page = 1;
    private int _pageSize = 10;
    private string _sortColumn = nameof(QuotationPlanListItemDto.PlanName);

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
        set => _sortColumn = _validSortColumns.Contains(value) ? value : nameof(QuotationPlanListItemDto.PlanName);
    }

    public bool SortDescending { get; set; }

    public string? SearchText { get; set; }

    public bool? ActiveOnly { get; set; }

    public DateTime? EffectiveDate { get; set; }
}
