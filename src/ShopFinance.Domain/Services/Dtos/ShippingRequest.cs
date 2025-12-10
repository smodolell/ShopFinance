namespace ShopFinance.Domain.Services.Dtos;

public class ShippingRequest
{
    public Guid WarehouseId { get; set; }
    public Address Destination { get; set; } = null!;
    public List<ShippingItem> Items { get; set; } = new();
    public decimal OrderTotal { get; set; }
    public DateTime RequestDate { get; set; } = DateTime.UtcNow;
    public ShippingPriority Priority { get; set; } = ShippingPriority.Standard;
    public Guid? CustomerId { get; set; }
}
