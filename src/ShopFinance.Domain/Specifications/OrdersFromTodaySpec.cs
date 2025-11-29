using ShopFinance.Domain.Entities;

namespace ShopFinance.Domain.Specifications;

public class OrdersFromTodaySpec : Specification<Order>
{
    public OrdersFromTodaySpec(DateTime today)
    {
        Query.Where(o => o.OrderDate >= today);
    }
}
