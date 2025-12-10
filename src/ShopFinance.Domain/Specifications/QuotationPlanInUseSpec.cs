using ShopFinance.Domain.Entities;

namespace ShopFinance.Domain.Specifications;

public class QuotationPlanInUseSpec : Specification<QuotationPlan>
{
    public QuotationPlanInUseSpec(int quotationPlanId)
    {
        Query.Where(qp => qp.Id == quotationPlanId)
             .Include(qp => qp.Phases);
    }
}
