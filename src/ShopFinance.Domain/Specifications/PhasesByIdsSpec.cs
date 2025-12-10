using ShopFinance.Domain.Entities;

namespace ShopFinance.Domain.Specifications;

public class PhasesByIdsSpec : Specification<Phase>
{
    public PhasesByIdsSpec(List<int> phaseIds)
    {
        Query.Where(p => phaseIds.Contains(p.Id));
    }
}
