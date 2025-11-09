using ShopFinance.Application.Features.Products.Commands;
using ShopFinance.Application.Features.Products.DTOs;
using ShopFinance.Domain.Entities;

namespace ShopFinance.Application.Features.Products;

public class ProductsMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Product, ProductListItemDto>()
            .Map(o => o.CategoryName, d => d.Category.Name);
        config.NewConfig<Product, ProductDetailDto>()
            .Map(o => o.ProductId, d => d.Id);

        config.NewConfig<CreateProductCommand, Product>();
        config.NewConfig<UpdateProductCommand, Product>();

    }
}
