namespace ShopFinance.Domain.Entities;

public class WarehouseProduct : BaseEntityAudit<Guid>
{
    public Guid WarehouseId { get; set; }
    public Guid ProductId { get; set; }
    public int StockQuantity { get; set; }
    public int StockMin { get; set; }
    public int StockMax { get; set; }
    public string? Location { get; set; } // Ej: "Estante A-12"

    // Navigation properties
    public Warehouse Warehouse { get; set; } = null!;
    public Product Product { get; set; } = null!;
}
