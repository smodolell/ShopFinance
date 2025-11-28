using ShopFinance.Domain.Entities;

namespace ShopFinance.Domain.Specifications;

public class CategorySpec : Specification<Category>
{

    public CategorySpec(string? searchText, DateTime? createdFrom, DateTime? createdTo)
    {
        if (!string.IsNullOrWhiteSpace(searchText))
        {
            Query.Where(u => u.Code.Contains(searchText) || u.Name.Contains(searchText));
        }
        if (createdFrom.HasValue)
        {
            Query.Where(r => r.CreatedAt >= createdFrom.Value);
        }

        if (createdTo.HasValue)
        {
            Query.Where(r => r.CreatedAt <= createdTo.Value);
        }
    }
}
