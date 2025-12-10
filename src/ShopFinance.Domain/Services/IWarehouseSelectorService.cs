using ShopFinance.Domain.Services.Dtos;

namespace ShopFinance.Domain.Services;

public interface IWarehouseSelectorService
{
    Task<WarehouseSelectionResult> SelectWarehouseAsync(
        List<ProductStockRequest> products,
        SaleContext context);
}
