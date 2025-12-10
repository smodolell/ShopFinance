using ShopFinance.Domain.Entities;

namespace ShopFinance.Domain.Specifications;

public class WarehouseByCodeSpec : Specification<Warehouse>
{
    public WarehouseByCodeSpec(string code)
    {
        Query.Where(w => w.Code == code && w.IsActive);
    }
}