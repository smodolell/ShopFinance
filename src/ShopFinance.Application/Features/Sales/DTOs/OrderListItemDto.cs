using ShopFinance.Domain.Enums;

namespace ShopFinance.Application.Features.Sales.DTOs;

public class OrderListItemDto
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public DateTime? RequiredDate { get; set; }
    public OrderStatus Status { get; set; }
    public decimal TotalAmount { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerIdentifier { get; set; }
    public int ItemsCount { get; set; }

    // Propiedades calculadas
    public bool IsOverdue => RequiredDate.HasValue && RequiredDate.Value < DateTime.Today && Status != OrderStatus.Cancelled;
    public string StatusDisplay => Status.ToString();
}
