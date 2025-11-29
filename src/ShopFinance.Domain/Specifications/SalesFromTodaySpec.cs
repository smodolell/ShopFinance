using ShopFinance.Domain.Entities;

namespace ShopFinance.Domain.Specifications;

public class SalesFromTodaySpec : Specification<Sale>
{
    public SalesFromTodaySpec(DateTime today)
    {
        Query.Where(o => o.SaleDate >= today);
    }
}
public class SaleByIdWithDetailsSpec : Specification<Sale>
{
    public SaleByIdWithDetailsSpec(Guid saleId)
    {
        Query.Where(o => o.Id == saleId);

        Query.Include(o => o.Order);
        Query.Include(o => o.Items)
             .ThenInclude(i => i.Product)
             .ThenInclude(p => p.Category);
    }
}
