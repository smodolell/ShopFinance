using ShopFinance.Domain.Enums;

namespace ShopFinance.Application.Features.Sales.DTOs;


public class SaleListItemDto
{
    public Guid Id { get; set; }
    public string SaleNumber { get; set; } = string.Empty;
    public DateTime SaleDate { get; set; }
    public SaleStatus Status { get; set; }
    public string? InvoiceNumber { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public decimal TotalAmount { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerIdentifier { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public int ItemsCount { get; set; }

    // Propiedades calculadas
    public string PaymentMethodDisplay => PaymentMethod.ToString();
    public string StatusDisplay => Status.ToString();
}
