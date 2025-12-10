using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Enums;

namespace ShopFinance.Domain.Specifications;

// 4. Specification para buscar por tipo
public class WarehouseByTypeSpec : Specification<Warehouse>
{
    public WarehouseByTypeSpec(WarehouseType type)
    {
        Query.Where(w => w.Type == type && w.IsActive);
    }
}
