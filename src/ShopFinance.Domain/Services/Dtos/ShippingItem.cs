namespace ShopFinance.Domain.Services.Dtos;

public class ShippingItem
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Weight { get; set; } // en kg
    public decimal Volume { get; set; } // en m³
    public decimal UnitPrice { get; set; }
    public bool IsFragile { get; set; }
}
