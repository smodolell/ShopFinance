using ShopFinance.Domain.Entities;

namespace ShopFinance.Domain.Specifications;

// 7. Specification para verificar si un producto está en un almacén
public class ProductInWarehouseSpec : Specification<WarehouseProduct>
{
    public ProductInWarehouseSpec(Guid warehouseId, Guid productId, int requiredQuantity)
    {
        Query.Where(wp => wp.WarehouseId == warehouseId &&
                         wp.ProductId == productId &&
                         wp.StockQuantity >= requiredQuantity);
    }
}