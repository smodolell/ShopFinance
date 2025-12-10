using ShopFinance.Domain.Entities;

namespace ShopFinance.Domain.Specifications;

public class PhaseByCodeSpec : Specification<Phase>
{
    public PhaseByCodeSpec(string code, bool includeStates = false)
    {
        Query.Where(p => p.Code == code);

        if (includeStates)
        {
            Query.Include(p => p.States);
        }
    }
}
