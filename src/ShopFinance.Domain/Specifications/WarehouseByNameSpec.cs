using ShopFinance.Domain.Entities;

namespace ShopFinance.Domain.Specifications;

public class WarehouseByNameSpec : Specification<Warehouse>
{
    public WarehouseByNameSpec(string searchText)
    {
        Query.Where(w => w.Name.Contains(searchText) && w.IsActive);
    }
}