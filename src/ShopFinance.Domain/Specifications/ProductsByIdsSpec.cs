using ShopFinance.Domain.Entities;

namespace ShopFinance.Domain.Specifications;

public class ProductsByIdsSpec : Specification<Product>
{
    public ProductsByIdsSpec(List<Guid> ids)
    {
        Query.Where(u => ids.Contains(u.Id));
    }
}
