using ShopFinance.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFinance.Application.Features.Sales.DTOs;


public class OrderViewDto
{
    public Guid OrderId { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public DateTime? RequiredDate { get; set; }
    public OrderStatus Status { get; set; }
    public decimal TotalAmount { get; set; }
    public string? Notes { get; set; }
    public Guid? CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerIdentifier { get; set; } = string.Empty;
    public List<OrderItemViewDto> Items { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Propiedades calculadas
    public bool IsOverdue => RequiredDate.HasValue &&
                           RequiredDate.Value < DateTime.Today &&
                           Status != OrderStatus.Cancelled;
    public int TotalItems => Items.Sum(i => i.Quantity);
}

public class OrderItemViewDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public int Stock { get; set; }
    public decimal SubTotal { get; set; }
}
