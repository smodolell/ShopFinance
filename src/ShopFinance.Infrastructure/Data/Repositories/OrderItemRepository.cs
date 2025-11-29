using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Repositories;
namespace ShopFinance.Infrastructure.Data.Repositories;

public class OrderItemRepository : Repository<OrderItem, Guid>, IOrderItemRepository
{
    public OrderItemRepository(ApplicationDbContext context) : base(context)
    {
    }
}
