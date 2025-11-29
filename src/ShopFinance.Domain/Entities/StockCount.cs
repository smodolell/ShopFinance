namespace ShopFinance.Domain.Entities;

public class StockCount : BaseEntityAudit<Guid>
{
    public string CountNumber { get; set; } = string.Empty;
    public Guid WarehouseId { get; set; }
    public StockCountStatus Status { get; set; }
    public DateTime CountDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public string? Notes { get; set; }

    // Navigation properties
    public Warehouse Warehouse { get; set; } = null!;
    public virtual ICollection<StockCountItem> Items { get; set; } = new List<StockCountItem>();
}
