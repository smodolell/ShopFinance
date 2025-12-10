using ShopFinance.Domain.Entities;

namespace ShopFinance.Domain.Specifications;


public class PhasesWithDetailsSpec : Specification<Phase>
{
    public PhasesWithDetailsSpec(int phaseId)
    {
        Query.Where(p => p.Id == phaseId);
        Query.Include(p => p.States);
        Query.OrderBy(p => p.Order);
    }
}
