using ShopFinance.Domain.Entities;

namespace ShopFinance.Domain.Specifications;

public class TaxRateSpec : Specification<TaxRate>
{
    public TaxRateSpec(
        string? searchText,
        bool? isActive,
        DateTime? effectiveDateFrom,
        DateTime? effectiveDateTo)
    {
        if (!string.IsNullOrWhiteSpace(searchText))
        {
            Query.Where(tr =>
                tr.Name.Contains(searchText) ||
                tr.Code.Contains(searchText) ||
                tr.Percentage.ToString().Contains(searchText));
        }

        if (isActive.HasValue)
        {
            Query.Where(tr => tr.IsActive == isActive.Value);
        }

        if (effectiveDateFrom.HasValue)
        {
            Query.Where(tr => tr.EffectiveDate >= effectiveDateFrom.Value);
        }

        if (effectiveDateTo.HasValue)
        {
            Query.Where(tr => tr.EffectiveDate <= effectiveDateTo.Value);
        }

        // Filtrar tasas vigentes (si se requiere)
        // Query.Where(tr => tr.ExpirationDate == null || tr.ExpirationDate >= DateTime.Today);

        // Orden por defecto
        Query.OrderByDescending(tr => tr.EffectiveDate)
             .ThenBy(tr => tr.Code);
    }
}