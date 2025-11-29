using ShopFinance.Domain.Entities;

namespace ShopFinance.Domain.Specifications;

public class WarehouseProductsWithLowStockSpec : Specification<WarehouseProduct>
{
    public WarehouseProductsWithLowStockSpec(Guid? warehouseId = null)
    {
        if (warehouseId.HasValue)
        {
            Query.Where(wp => wp.WarehouseId == warehouseId.Value && wp.StockQuantity <= wp.StockMin);
        }
        else
        {
            Query.Where(wp => wp.StockQuantity <= wp.StockMin);
        }
        Query.Include(wp => wp.Product)
            .Include(wp => wp.Warehouse);
    }

}
