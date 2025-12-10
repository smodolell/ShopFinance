using ShopFinance.Domain.Entities;

namespace ShopFinance.Domain.Specifications;

// 3. Specification para buscar almacenes de una tienda por nombre
public class WarehouseByStoreNameSpec : Specification<Warehouse>
{
    public WarehouseByStoreNameSpec(Guid storeId)
    {
        Query.Where(w => (w.Name.Contains($"Tienda {storeId}") ||
                         w.Name.Contains($"Store {storeId}") ||
                         w.Name.Contains(storeId.ToString().Substring(0, 8))) &&
                        w.IsActive);
    }
}
