using ShopFinance.Domain.Entities;

namespace ShopFinance.Domain.Specifications;

public class UserSpec : Specification<User>
{

    public UserSpec(string? searchText)
    {
        if (!string.IsNullOrWhiteSpace(searchText))
        {
            Query.Where(u =>
                (u.UserName != null && u.UserName.Contains(searchText)) ||
                u.FullName.Contains(searchText)
            );
        }

    }
}
