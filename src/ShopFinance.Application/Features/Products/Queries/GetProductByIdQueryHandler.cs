using ShopFinance.Application.Features.Products.DTOs;

namespace ShopFinance.Application.Features.Products.Queries;

internal class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, Result<ProductDetailDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetProductByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ProductDetailDto>> HandleAsync(GetProductByIdQuery message, CancellationToken cancellationToken = default)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(message.ProductId);

        if (product == null)
        {
            return Result.NotFound("NO existe");
        }
        var result = product.Adapt<ProductDetailDto>();

        return Result.Success(result);
    }
}