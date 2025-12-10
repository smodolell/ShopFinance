namespace ShopFinance.Domain.Services.Dtos;

public class WarehouseAllocation
{
    public Guid ProductId { get; set; }
    public Guid WarehouseId { get; set; }
    public string WarehouseName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string? Location { get; set; }
    public decimal ShippingCost { get; set; }
    public int EstimatedDays { get; set; }
}
