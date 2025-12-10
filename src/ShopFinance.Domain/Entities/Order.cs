using ShopFinance.Domain.Enums;

namespace ShopFinance.Domain.Entities;

public class Order : BaseEntityAudit<Guid>
{
    public Guid? CustomerId { get; set; }
    public Guid? QuotationId { get; set; }
    
    public OrderStatus Status { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public string? Notes { get; set; }
    public DateTime? RequiredDate { get; set; } // Fecha requerida de entrega
    public decimal TotalAmount { get; set; }
    public bool CanBeConfirmed() => Status == OrderStatus.Draft;
    public bool CanBeCancelled() => Status != OrderStatus.Cancelled;

    public decimal CalculateTotal() => Items.Sum(x => x.Quantity * x.UnitPrice);

    public virtual Customer? Customer { get; set; }
    public virtual Sale? Sale { get; set; }
    public virtual Quotation? Quotation { get; set; }

    public virtual ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
 
}
