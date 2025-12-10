using ShopFinance.Domain.Enums;

namespace ShopFinance.Domain.Entities;

public class Sale : BaseEntityAudit<Guid>  // Agregada herencia
{
    public Guid OrderId { get; set; } // Referencia al pedido original
    public Guid? WarehouseId { get; set; }

    public SaleChannel SaleChannel { get; set; }

    public string SaleNumber { get; set; } = string.Empty;
    
    public DateTime SaleDate { get; set; }
    public SaleStatus Status { get; set; }
    public string? InvoiceNumber { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public decimal TotalAmount { get; set; }

    public Order Order { get; set; } = null!;

    public Warehouse? Warehouse { get; set; }
    public virtual ICollection<SaleItem> Items { get; set; } = new List<SaleItem>();
}
