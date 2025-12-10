using ShopFinance.Domain.Enums;
using ShopFinance.Domain.Services.Dtos;

namespace ShopFinance.Domain.Services.Implementations;

// Versión 1.0: Simple
public class SimpleWarehouseSelector : IWarehouseSelectorService
{
    public Task<WarehouseSelectionResult> SelectWarehouseAsync(
        List<ProductStockRequest> products, SaleContext context)
    {
        // REGLA SIMPLE: Siempre el almacén principal
        var mainWarehouseId = Guid.Parse("almacen-principal-id");

        return Task.FromResult(new WarehouseSelectionResult
        {
            Success = true,
            Allocations = products.Select(p => new WarehouseAllocation
            {
                ProductId = p.ProductId,
                WarehouseId = mainWarehouseId,
                WarehouseName = "Almacén Principal",
                Quantity = p.Quantity,
                ShippingCost = context.Channel == SaleChannel.Ecommerce ? 85 : 0,
                EstimatedDays = context.Channel == SaleChannel.Ecommerce ? 3 : 0
            }).ToList(),
            EstimatedShippingCost = context.Channel == SaleChannel.Ecommerce ? 85 : 0,
            EstimatedDeliveryTime = context.Channel == SaleChannel.Ecommerce
                ? TimeSpan.FromDays(3)
                : TimeSpan.Zero
        });
    }
}