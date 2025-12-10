using ShopFinance.Domain.Enums;

namespace ShopFinance.Domain.Services.Dtos;

public class SaleContext
{
    public Guid? StoreId { get; set; }
    public SaleChannel Channel { get; set; }
    public string? MarketplaceName { get; set; }
    public string? DeliveryMethod { get; set; }
    public string? ShippingAddress { get; set; }
    public Guid? CustomerId { get; set; }
    public decimal OrderTotal { get; set; }
    public DateTime RequestDate { get; set; } = DateTime.UtcNow;
}