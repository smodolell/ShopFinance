using ShopFinance.Domain.Services;
using ShopFinance.Domain.Services.Dtos;

public class SimpleShippingCalculator : IShippingCalculator
{
    public Task<decimal> CalculateCostAsync(ShippingRequest request)
    {
        // Versión 1.0: Tarifa plana
        return Task.FromResult(85.00m); // Siempre $85
    }

    public Task<int> EstimateDeliveryDaysAsync(ShippingRequest request)
    {
        // Versión 1.0: Siempre 3 días
        return Task.FromResult(3);
    }

    public Task<ShippingOption[]> GetAvailableShippingOptionsAsync(ShippingRequest request)
    {
        // Versión 1.0: Una sola opción
        var option = new ShippingOption
        {
            Carrier = "ShopFinance",
            Service = "Estándar",
            Cost = 85.00m,
            EstimatedDays = 3,
            EstimatedDeliveryDate = DateTime.Now.AddDays(3),
            Trackable = true,
            Insured = false
        };

        return Task.FromResult(new[] { option });
    }
}