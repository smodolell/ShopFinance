namespace ShopFinance.Application.Features.Sales.DTOs;

public class SaleItemDto
{

    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal CostPrice { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal SubTotal => Quantity * UnitPrice;
    public decimal Profit => SubTotal - (CostPrice * Quantity);
    public decimal ProfitMargin => UnitPrice > 0 ? (Profit / (CostPrice * Quantity)) * 100 : 0;
}
