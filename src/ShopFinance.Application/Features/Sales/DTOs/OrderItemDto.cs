namespace ShopFinance.Application.Features.Sales.DTOs;

public class OrderItemDto
{
    public Guid ProductId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public int Stock { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal SubTotal => Quantity * UnitPrice;
}
