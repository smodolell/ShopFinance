using ShopFinance.Domain.Entities;

namespace ShopFinance.Domain.Specifications;

public class FrequenciesByIdsSpec : Specification<Frequency>
{
    public FrequenciesByIdsSpec(List<int> ids)
    {
        Query.Where(p => ids.Contains(p.Id));
    }
}
