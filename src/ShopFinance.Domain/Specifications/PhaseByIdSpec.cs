using ShopFinance.Domain.Entities;

namespace ShopFinance.Domain.Specifications;



public class PhaseByIdSpec : Specification<Phase>
{
    public PhaseByIdSpec(int id, bool includeStates = false)
    {
        Query.Where(p => p.Id == id);

        if (includeStates)
        {
            Query.Include(p => p.States);
        }
    }
}

