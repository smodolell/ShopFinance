using ShopFinance.Application.Features.Categories.DTOs;
using ShopFinance.Domain.Entities;

namespace ShopFinance.Application.Features.Categories;

public class CategoriesMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Category, CategoryDto>();
        config.NewConfig<CategoryDto, Category>();
        
    }
}
