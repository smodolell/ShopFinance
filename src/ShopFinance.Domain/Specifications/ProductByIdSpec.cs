using ShopFinance.Domain.Entities;

namespace ShopFinance.Domain.Specifications;

public class ProductByIdSpec : Specification<Product>
{
    public ProductByIdSpec(Guid id)
    {
        Query.Where(u => u.Id == id);
    }
}
