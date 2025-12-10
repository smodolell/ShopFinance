using ShopFinance.Domain.Entities;

namespace ShopFinance.Domain.Specifications;

public class QuotationPlanByNameSpec : Specification<QuotationPlan>
{
    public QuotationPlanByNameSpec(string planName)
    {
        Query.Where(qp => qp.PlanName == planName);
    }
}
