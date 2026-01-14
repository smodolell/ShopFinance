using ShopFinance.Domain.Entities;

namespace ShopFinance.Domain.Specifications;
public class InterestRatesByIdsSpec : Specification<InterestRate>
{
    public InterestRatesByIdsSpec(List<int> ids)
    {
        Query.Where(p => ids.Contains(p.Id));
    }
}
