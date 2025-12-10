using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Enums;

namespace ShopFinance.Domain.Specifications;

// 2. Specification para buscar por tipo y código exacto
public class WarehouseByTypeAndCodeSpec : Specification<Warehouse>
{
    public WarehouseByTypeAndCodeSpec(WarehouseType type, string code)
    {
        Query.Where(w => w.Type == type && w.Code == code && w.IsActive);
    }
}
