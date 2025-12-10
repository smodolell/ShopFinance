using ShopFinance.Domain.Entities;

namespace ShopFinance.Domain.Specifications;

public class QuotationPlanByIdSpec : Specification<QuotationPlan>
{
    public QuotationPlanByIdSpec(int id, bool includePhases = false)
    {
        Query.Where(qp => qp.Id == id);

        if (includePhases)
        {
            Query.Include(qp => qp.Phases)
                 .ThenInclude(qpp => qpp.Phase);
        }
    }
}
