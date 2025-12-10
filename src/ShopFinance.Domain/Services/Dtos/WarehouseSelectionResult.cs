namespace ShopFinance.Domain.Services.Dtos;

public class WarehouseSelectionResult
{
    public bool Success { get; set; }
    public List<WarehouseAllocation> Allocations { get; set; } = new();
    public string? ErrorMessage { get; set; }
    public decimal EstimatedShippingCost { get; set; }
    public TimeSpan EstimatedDeliveryTime { get; set; }
}
