using ShopFinance.Domain.Common.Interfaces;

namespace ShopFinance.Application.Features.Products.Commands;

public class CreateProductCommand : ICommand<Result<Guid>>
{

    public int? CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal CostPrice { get; set; }
    public decimal SalePrice { get; set; }
    public int Stock { get; set; }
    public int StockMin { get; set; }
}
