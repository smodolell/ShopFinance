using ShopFinance.Domain.Entities;

namespace ShopFinance.Domain.Specifications;

public class RoleSpec : Specification<Role>
{
    public string? SearchText { get; set; }

    public RoleSpec(string? searchText)
    {
        SearchText = searchText;

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            Query.Where(u => (u.Name != null && u.Name.Contains(SearchText)) || u.Description.Contains(SearchText));
        }

    }
}
