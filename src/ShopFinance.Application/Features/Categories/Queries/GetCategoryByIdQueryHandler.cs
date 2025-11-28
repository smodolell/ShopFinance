using ShopFinance.Application.Features.Categories.DTOs;

namespace ShopFinance.Application.Features.Categories.Queries;

internal class GetCategoryByIdQueryHandler : IQueryHandler<GetCategoryByIdQuery, Result<CategoryViewDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCategoryByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<CategoryViewDto>> HandleAsync(GetCategoryByIdQuery message, CancellationToken cancellationToken = default)
    {

        var category = await _unitOfWork.Categories.GetByIdAsync(message.CategoryId, cancellationToken);
        if (category == null)
        {
            return Result.NotFound("NO existe");
        }
        var result = category.Adapt<CategoryViewDto>();
        return Result.Success(result);
    }
}