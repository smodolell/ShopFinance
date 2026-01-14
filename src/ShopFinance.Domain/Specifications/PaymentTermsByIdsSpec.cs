using ShopFinance.Domain.Entities;

namespace ShopFinance.Domain.Specifications;

public class PaymentTermsByIdsSpec : Specification<PaymentTerm>
{
    public PaymentTermsByIdsSpec(List<int> ids)
    {
        Query.Where(p => ids.Contains(p.Id));
    }
}
