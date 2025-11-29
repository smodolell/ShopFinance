using ShopFinance.Domain.Entities;

namespace ShopFinance.Domain.Specifications;

public class OrderByIdWithDetailsSpec : Specification<Order>
{
    public OrderByIdWithDetailsSpec(Guid orderId)
    {
        Query.Where(o => o.Id == orderId);

        // Incluir todas las relaciones necesarias
        Query.Include(o => o.Customer);
        Query.Include(o => o.Items)
             .ThenInclude(i => i.Product)
             .ThenInclude(p => p.Category);
    }
}