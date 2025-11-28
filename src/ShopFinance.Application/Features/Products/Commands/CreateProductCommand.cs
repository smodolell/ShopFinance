using ShopFinance.Domain.Common.Interfaces;
using ShopFinance.Domain.Enums;

namespace ShopFinance.Application.Features.Products.Commands;

public class CreateProductCommand : ICommand<Result<Guid>>
{

    public int? CategoryId { get; set; }
    public ProductState? State { get; set; }
    public string Code { get; set; } = string.Empty;
    public string CodeSku { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal CostPrice { get; set; }
    public decimal SalePrice { get; set; }
    public int Stock { get; set; }
    public int StockMin { get; set; }
    public int StockMax { get; set; }
}
