using ShopFinance.Domain.Enums;

namespace ShopFinance.Domain.Entities;

public class Product : BaseEntityAudit<Guid>
{
    public int CategoryId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string CodeSku { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal CostPrice { get; set; }
    public decimal SalePrice { get; set; }
    public int Stock { get; set; }
    public int StockMin { get; set; }
    public ProductState State { get; set; }

    public virtual Category Category { get; set; } = null!;
 
    public virtual ICollection<WarehouseProduct> WarehouseProducts { get; set; } = new List<WarehouseProduct>();
    public virtual ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
    public virtual ICollection<StockCountItem> StockCountItems { get; set; } = new List<StockCountItem>();
    public virtual ICollection<StockTransferItem> StockTransferItems { get; set; } = new List<StockTransferItem>();
    public virtual ICollection<StockAlert> StockAlerts { get; set; } = new List<StockAlert>();
    public virtual ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
