using ShopFinance.Domain.Enums;

namespace ShopFinance.Application.Features.Products.DTOs;

public class ProductForSaleDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? CodeSku { get; set; }
    public decimal CostPrice { get; set; }
    public decimal SalePrice { get; set; }
    public int Stock { get; set; }
    public int StockMin { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public ProductState State { get; set; }

    // Propiedades calculadas
    public bool IsLowStock => Stock <= StockMin;
    public bool CanBeSold => Stock > 0 && State == ProductState.Active;
}
