using ShopFinance.Domain.Entities;

namespace ShopFinance.Domain.Specifications;

public class CustomerSpec : Specification<Customer>
{
    public CustomerSpec(string? searchText)
    {
        if (!string.IsNullOrWhiteSpace(searchText))
        {
            searchText = searchText.Trim().ToLower();
            Query.Where(u =>
                u.FirstName.ToLower().Contains(searchText) ||
                u.LastName.ToLower().Contains(searchText) ||
                u.Identifier.Contains(searchText)
            );
        }
    }
}
