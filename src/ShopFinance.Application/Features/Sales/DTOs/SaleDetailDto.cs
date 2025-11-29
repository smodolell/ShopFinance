using ShopFinance.Domain.Enums;

namespace ShopFinance.Application.Features.Sales.DTOs;

public class SaleDetailDto
{
    public Guid Id { get; set; }
    public string SaleNumber { get; set; } = string.Empty;
    public DateTime SaleDate { get; set; }
    public SaleStatus Status { get; set; }
    public string? InvoiceNumber { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public decimal TotalAmount { get; set; }
    public Guid OrderId { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public Guid? CustomerId { get; set; }
    public string? CustomerName { get; set; } = string.Empty;
    public string? CustomerIdentifier { get; set; } = string.Empty;
    public List<SaleItemDto> Items { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Propiedades calculadas
    public decimal TotalProfit => Items.Sum(i => i.Profit);
    public int TotalItems => Items.Sum(i => i.Quantity);
    public string PaymentMethodDisplay => PaymentMethod.ToString();
    public string StatusDisplay => Status.ToString();
}
