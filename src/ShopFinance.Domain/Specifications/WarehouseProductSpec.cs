using ShopFinance.Domain.Entities;

namespace ShopFinance.Domain.Specifications;

public class WarehouseProductSpec : Specification<WarehouseProduct>
{
    public WarehouseProductSpec(Guid warehouseId, Guid productId)
    {
        Query.Where(u => u.Id == warehouseId && u.ProductId == productId);
    }
}

