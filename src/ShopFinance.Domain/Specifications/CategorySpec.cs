using ShopFinance.Domain.Entities;

namespace ShopFinance.Domain.Specifications;

public class CategorySpec : Specification<Category>
{
    public string? SearchText { get; set; }

    public CategorySpec()
    {
        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            Query.Where(u => u.Code.Contains(SearchText) || u.Name.Contains(SearchText));
        }

    }
}
