using ShopFinance.Domain.Entities;

namespace ShopFinance.Domain.Specifications;

public class InterestRateSpec : Specification<InterestRate>
{
    public InterestRateSpec(string? searchText, bool? isActive, DateTime? effectiveDate = null)
    {
        if (!string.IsNullOrWhiteSpace(searchText))
        {
            Query.Where(u =>
                u.RateName.Contains(searchText) ||
                u.AnnualPercentage.ToString().Contains(searchText));
        }

        if (isActive.HasValue)
        {
            Query.Where(r => r.IsActive == isActive.Value);
        }

        if (effectiveDate.HasValue)
        {
            Query.Where(r =>
                r.EffectiveDate <= effectiveDate.Value &&
                (!r.ExpirationDate.HasValue || r.ExpirationDate >= effectiveDate.Value));
        }

        // Orden por defecto
        Query.OrderBy(r => r.RateName);
    }
}