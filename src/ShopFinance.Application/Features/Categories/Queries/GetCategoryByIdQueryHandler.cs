using ShopFinance.Application.Features.Categories.DTOs;

namespace ShopFinance.Application.Features.Categories.Queries;

internal class GetCategoryByIdQueryHandler : IQueryHandler<GetCategoryByIdQuery, Result<CategoryDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCategoryByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<CategoryDto>> HandleAsync(GetCategoryByIdQuery message, CancellationToken cancellationToken = default)
    {

        var category = await _unitOfWork.Categories.GetByIdAsync(message.CategoryId, cancellationToken);
        if (category == null)
        {
            return Result.NotFound("NO existe");
        }
        var result = category.Adapt<CategoryDto>();
        return Result.Success(result);
    }
}