namespace ShopFinance.Domain.Entities;

public class StockTransferItem : BaseEntity<Guid>
{
    public Guid StockTransferId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public int QuantitySent { get; set; }
    public int QuantityReceived { get; set; }
    public string? Notes { get; set; }

    // Navigation properties
    public StockTransfer StockTransfer { get; set; } = null!;
    public Product Product { get; set; } = null!;
}

