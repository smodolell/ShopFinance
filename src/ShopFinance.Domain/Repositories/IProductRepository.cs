using ShopFinance.Domain.Entities;

namespace ShopFinance.Domain.Repositories;

public interface IProductRepository : IRepository<Product, Guid>
{
}
