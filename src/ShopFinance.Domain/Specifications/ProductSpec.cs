using ShopFinance.Domain.Entities;

namespace ShopFinance.Domain.Specifications;

public class ProductSpec : Specification<Product>
{
    public string? SearchText { get; set; }

    public ProductSpec()
    {
        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            Query.Where(u => u.Name.Contains(SearchText) || u.Description.Contains(SearchText));
        }

    }
}
