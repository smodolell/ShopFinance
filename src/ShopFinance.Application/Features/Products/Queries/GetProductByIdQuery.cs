using ShopFinance.Application.Features.Products.DTOs;

namespace ShopFinance.Application.Features.Products.Queries;

public class GetProductByIdQuery : IQuery<Result<ProductDetailDto>>
{
    public Guid ProductId { get; set; }
}
