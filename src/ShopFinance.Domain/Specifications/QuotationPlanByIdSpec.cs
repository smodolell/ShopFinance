using ShopFinance.Domain.Entities;

namespace ShopFinance.Domain.Specifications;

public class QuotationPlanByIdSpec : Specification<QuotationPlan>
{
    public QuotationPlanByIdSpec(int id, bool includePhases = false, bool includeFrequencies = false, bool includePaymentTerms = false)
    {
        Query.Where(qp => qp.Id == id);

        if (includePhases)
        {
            Query.Include(qp => qp.Phases)
                 .ThenInclude(qpp => qpp.Phase);
        }
        if (includeFrequencies)
        {
            Query.Include(qp => qp.Frequencies)
                 .ThenInclude(qpf => qpf.Frequency);
        }
        if (includePaymentTerms)
        {
            Query.Include(qp => qp.PaymentTerms)
                 .ThenInclude(qppt => qppt.PaymentTerm);
        }
    }
}
