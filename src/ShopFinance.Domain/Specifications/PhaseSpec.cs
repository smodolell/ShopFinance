using ShopFinance.Domain.Entities;

namespace ShopFinance.Domain.Specifications;

public class PhaseSpec : Specification<Phase>
{
    public PhaseSpec(string? searchText, bool? isInitial, bool? isFinal, bool? required)
    {
        if (!string.IsNullOrWhiteSpace(searchText))
        {
            searchText = searchText.Trim().ToLower();
            Query.Where(u => u.Code.ToLower().Contains(searchText) ||
                            u.PhaseName.ToLower().Contains(searchText));
        }
        if (isInitial.HasValue)
        {
            Query.Where(r => r.IsInitial == isInitial.Value);
        }

        if (isFinal.HasValue)
        {
            Query.Where(r => r.IsFinal == isFinal.Value);
        }

        if (required.HasValue)
        {
            Query.Where(r => r.Required == required.Value);
        }
        Query.Include(p => p.States);

        //Query.OrderBy(p => p.Order);
    }
}