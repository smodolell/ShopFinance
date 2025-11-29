using ShopFinance.Domain.Enums;

namespace ShopFinance.Domain.Entities;

public class Warehouse : BaseEntityAudit<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public bool IsActive { get; set; } = true;
    public WarehouseType Type { get; set; } = WarehouseType.Physical;

    // Navigation properties
    public virtual ICollection<WarehouseProduct> Products { get; set; } = new List<WarehouseProduct>();
    public virtual ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
    public virtual ICollection<StockCount> StockCounts { get; set; } = new List<StockCount>();
    public virtual ICollection<StockAlert> StockAlerts { get; set; } = new List<StockAlert>();
}
