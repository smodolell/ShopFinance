using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Repositories;
namespace ShopFinance.Infrastructure.Data.Repositories;

public class OrderRepository : Repository<Order, Guid>, IOrderRepository 
{
    public OrderRepository(ApplicationDbContext context) : base(context)
    {
    }
}


public class StockTransferRepository : Repository<StockTransfer, Guid>, IStockTransferRepository
{
    public StockTransferRepository(ApplicationDbContext context) : base(context)
    {
    }
}

public class WarehouseProductRepository : Repository<WarehouseProduct, Guid>, IWarehouseProductRepository
{
    public WarehouseProductRepository(ApplicationDbContext context) : base(context)
    {
    }
}
public class StockMovementRepository : Repository<StockMovement, Guid>, IStockMovementRepository
{
    public StockMovementRepository(ApplicationDbContext context) : base(context)
    {
    }
}
