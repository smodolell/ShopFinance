namespace ShopFinance.Domain.Entities;

public class StockAlert : BaseEntity<Guid>
{
    public Guid ProductId { get; set; }
    public Guid WarehouseId { get; set; }
    public StockAlertType AlertType { get; set; }
    public int CurrentStock { get; set; }
    public int Threshold { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime AlertDate { get; set; }
    public Warehouse Warehouse { get; set; } = null!;
    public Product Product { get; set; } = null!;
}
