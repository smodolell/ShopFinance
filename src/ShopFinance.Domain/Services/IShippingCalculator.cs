using ShopFinance.Domain.Services.Dtos;

namespace ShopFinance.Domain.Services;

public interface IShippingCalculator
{
    Task<decimal> CalculateCostAsync(ShippingRequest request);
    Task<int> EstimateDeliveryDaysAsync(ShippingRequest request);
    Task<ShippingOption[]> GetAvailableShippingOptionsAsync(ShippingRequest request);
}
