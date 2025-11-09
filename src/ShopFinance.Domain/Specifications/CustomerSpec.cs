using ShopFinance.Domain.Entities;

namespace ShopFinance.Domain.Specifications;

public class CustomerSpec : Specification<Customer>
{
    public CustomerSpec(string? searchText)
    {
        if (!string.IsNullOrWhiteSpace(searchText))
        {
            Query.Where(u =>
                u.FirstName.Contains(searchText) ||
                u.LastName.Contains(searchText) ||
                u.Identifier.Contains(searchText)
            );
        }
    }
}
