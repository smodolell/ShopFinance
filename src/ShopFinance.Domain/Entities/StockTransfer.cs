using ShopFinance.Domain.Enums;

namespace ShopFinance.Domain.Entities;

public class StockTransfer : BaseEntityAudit<Guid>
{
    public string TransferNumber { get; set; } = string.Empty;
    public Guid FromWarehouseId { get; set; }
    public Guid ToWarehouseId { get; set; }
    public TransferStatus Status { get; set; }
    public DateTime TransferDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public string? Notes { get; set; }

    // Navigation properties
    public Warehouse FromWarehouse { get; set; } = null!;
    public Warehouse ToWarehouse { get; set; } = null!;
    public virtual ICollection<StockTransferItem> Items { get; set; } = new List<StockTransferItem>();
}

