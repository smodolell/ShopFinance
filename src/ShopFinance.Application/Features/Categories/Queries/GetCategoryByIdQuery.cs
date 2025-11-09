using ShopFinance.Application.Features.Categories.DTOs;

namespace ShopFinance.Application.Features.Categories.Queries;

public class GetCategoryByIdQuery : IQuery<Result<CategoryDto>>
{
    public int CategoryId { get; set; }
}
