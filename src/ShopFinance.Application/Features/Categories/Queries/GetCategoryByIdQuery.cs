using ShopFinance.Application.Features.Categories.DTOs;

namespace ShopFinance.Application.Features.Categories.Queries;

public class GetCategoryByIdQuery : IQuery<Result<CategoryViewDto>>
{
    public int CategoryId { get; set; }
}
