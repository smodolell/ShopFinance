using ShopFinance.Domain.Entities;

namespace ShopFinance.Domain.Specifications;

public class CustomerByIdentifierSpec : Specification<Customer>
{
    public CustomerByIdentifierSpec(string identifier)
    {
        Query.Where(c => c.Identifier == identifier);
    }
}