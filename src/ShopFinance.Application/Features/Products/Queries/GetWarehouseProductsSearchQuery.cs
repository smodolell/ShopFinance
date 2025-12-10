using Microsoft.EntityFrameworkCore;
using ShopFinance.Application.Features.Products.DTOs;
using ShopFinance.Domain.Specifications;

namespace ShopFinance.Application.Features.Products.Queries;

public class GetWarehouseProductsSearchQuery : IQuery<PagedResult<List<ProductSearchDto>>>
{
    private static readonly HashSet<string> _validSortColumns = new()
    {
        nameof(ProductSearchDto.Code),
        nameof(ProductSearchDto.CodeSku),
        nameof(ProductSearchDto.Stock ),
    };

    private int _page = 1;
    private int _pageSize = 10;
    private string _sortColumn = nameof(ProductSearchDto.Stock);

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
        set => _sortColumn = _validSortColumns.Contains(value) ? value : nameof(ProductSearchDto.Stock);
    }
    public bool SortDescending { get; set; }
    public string? SearchText { get; set; }
    public int? CategoryId { get; set; }
    public List<Guid> WarehouseIds { get; set; } = new();
    public bool SumStock { get; set; } = true;
}


internal class GetWarehouseProductsSearchQueryHandler : IQueryHandler<GetWarehouseProductsSearchQuery, PagedResult<List<ProductSearchDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDynamicSorter _sorter;
    private readonly IPaginator _paginator;

    public GetWarehouseProductsSearchQueryHandler(IUnitOfWork unitOfWork, IDynamicSorter sorter, IPaginator paginator)
    {
        _unitOfWork = unitOfWork;
        _sorter = sorter;
        _paginator = paginator;
    }

    public async Task<PagedResult<List<ProductSearchDto>>> HandleAsync(GetWarehouseProductsSearchQuery message, CancellationToken cancellationToken = default)
    {
        var spec = new WarehouseProductForSaleSpec(
            message.SearchText,
            message.CategoryId,
            message.WarehouseIds
        );
        var query = _unitOfWork.WarehouseProducts.ApplySpecification(spec);
        query = _sorter.ApplySort(query, message.SortColumn, message.SortDescending);
        query.GroupBy(wp => new { wp.Product.Id, wp.Product.Name, wp.Product.Code, wp.Product.CodeSku });

        var total = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((message.Page - 1) * message.PageSize)
            .Take(message.PageSize)
            .AsNoTracking()
            .ToListAsync();
        var result = new List<ProductSearchDto>();

        if (message.SumStock)
        {
            result = items.GroupBy(wp => new { wp.Product.Id, wp.Product.Name, wp.Product.Code, wp.Product.CodeSku })
                .Select(g => new ProductSearchDto
                {
                    Id = g.Key.Id,
                    Name = g.Key.Name,
                    Code = g.Key.Code,
                    CodeSku = g.Key.CodeSku,
                    Stock = g.Sum(wp => wp.StockQuantity)
                }).ToList();

        }
        else
        {
            result = items.Select(s=> new ProductSearchDto
            {
                Id = s.Id,
                Name = s.Product.Name,
                Code = s.Product.Code,
                CodeSku = s.Product.CodeSku,
                Stock = s.StockQuantity,
                WarehouseId = s.WarehouseId,
            }).ToList();
        }

        return _paginator.CreatePagedResult(result, message.Page, message.PageSize, total);
    }
}
