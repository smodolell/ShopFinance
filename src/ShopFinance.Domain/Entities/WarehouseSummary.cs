namespace ShopFinance.Domain.Entities;

public class WarehouseSummary :BaseEntity<Guid>
{
    public Guid WarehouseId { get; set; }
    public int TotalProducts { get; set; }
    public int LowStockProducts { get; set; }
    public int OutOfStockProducts { get; set; }
    public decimal TotalInventoryValue { get; set; }
    public int TotalMovementsToday { get; set; }
}
