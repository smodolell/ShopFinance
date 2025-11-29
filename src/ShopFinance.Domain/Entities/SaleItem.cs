namespace ShopFinance.Domain.Entities;

public class SaleItem : BaseEntity<Guid>
{
    public Guid SaleId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal CostPrice { get; set; } // Precio de costo en el momento de la venta
    public decimal UnitPrice { get; set; } // Precio de venta

    // Navigation properties
    public Sale Sale { get; set; } = null!;
    public Product Product { get; set; } = null!;
}
