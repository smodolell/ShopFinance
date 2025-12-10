using ShopFinance.Domain.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ShopFinance.Domain.Specifications;


public class ActiveWarehousesSpec : Specification<Warehouse>
{
    public ActiveWarehousesSpec()
    {
        Query.Where(w => w.IsActive)
             .OrderBy(w => w.CreatedAt)
             .Take(1);
    }
}
