namespace ShopFinance.Domain.Entities;



public class StockCountItem : BaseEntity<Guid>
{
    public Guid StockCountId { get; set; }
    public Guid ProductId { get; set; }
    public int SystemQuantity { get; set; } // Cantidad en sistema
    public int PhysicalQuantity { get; set; } // Cantidad contada físicamente
    public int Variance => PhysicalQuantity - SystemQuantity; // Diferencia
    public string? Notes { get; set; }

    // Navigation properties
    public StockCount StockCount { get; set; } = null!;
    public Product Product { get; set; } = null!;
}