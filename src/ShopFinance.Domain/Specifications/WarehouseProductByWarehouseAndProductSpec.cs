using ShopFinance.Domain.Entities;

namespace ShopFinance.Domain.Specifications;

// 6. Specification para WarehouseProduct
public class WarehouseProductByWarehouseAndProductSpec : Specification<WarehouseProduct>
{
    public WarehouseProductByWarehouseAndProductSpec(Guid warehouseId, Guid productId)
    {
        Query.Where(wp => wp.WarehouseId == warehouseId && wp.ProductId == productId)
             .Include(wp => wp.Warehouse); // Incluir datos del almacén
    }
}
