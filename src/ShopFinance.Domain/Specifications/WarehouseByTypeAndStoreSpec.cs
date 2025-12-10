using Ardalis.Specification;
using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Enums;

namespace ShopFinance.Domain.Specifications;

// 1. Specification para buscar Warehouse por tipo y store code
public class WarehouseByTypeAndStoreSpec : Specification<Warehouse>
{
    public WarehouseByTypeAndStoreSpec(WarehouseType type, string storeIdentifier)
    {
        Query.Where(w => w.Type == type &&
                        (w.Code.Contains(storeIdentifier) ||
                         w.Name.Contains(storeIdentifier)) &&
                        w.IsActive);
    }
}
