using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Enums;

namespace ShopFinance.Domain.Specifications;

public class ProductSpec : Specification<Product>
{

    public ProductSpec(string? searchText, ProductState? state,int? categoryId)
    {
        if (!string.IsNullOrWhiteSpace(searchText))
        {
            searchText = searchText.ToLower();
            Query.Where(u => u.Code.ToLower().Contains(searchText) 
            || u.CodeSku.ToLower().Contains(searchText) 
            || u.Name.ToLower().Contains(searchText) 
            || u.Description.ToLower().Contains(searchText));
        }

        if (state != null)
        {
            Query.Where(u => u.State == state);
        }

        if(categoryId.HasValue)
        {
            Query.Where(u => u.CategoryId == categoryId.Value);
        }
    }
}
