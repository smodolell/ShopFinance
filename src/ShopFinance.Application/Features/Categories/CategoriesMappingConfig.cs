using ShopFinance.Application.Features.Categories.DTOs;
using ShopFinance.Domain.Entities;

namespace ShopFinance.Application.Features.Categories;

public class CategoriesMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Category, CategoryListItemDto>();
        config.NewConfig<Category, CategoryViewDto>()
            .Map(o => o.CategoryId, d => d.Id); ;
        
    }
}
