using ShopFinance.Domain.Entities;

namespace ShopFinance.Domain.Specifications;

public class QuotationPlanSpec : Specification<QuotationPlan>
{
    public QuotationPlanSpec(
        string? searchText = null,
        bool? activeOnly = null,
        DateTime? effectiveDate = null,
        bool includePhases = false)
    {
        // Filtro por texto de búsqueda
        if (!string.IsNullOrWhiteSpace(searchText))
        {
            searchText = searchText.Trim().ToLower();
            Query.Where(qp => qp.PlanName.ToLower().Contains(searchText));
        }

        // Filtro por estado activo
        if (activeOnly.HasValue && activeOnly.Value)
        {
            var now = DateTime.UtcNow;
            Query.Where(qp =>
                qp.InitialEffectiveDate <= now &&
                qp.FinalEffectiveDate >= now);
        }

        // Filtro por fecha de vigencia
        if (effectiveDate.HasValue)
        {
            var effectiveDateValue = effectiveDate.Value.Date;
            Query.Where(qp =>
                qp.InitialEffectiveDate.Date <= effectiveDateValue &&
                qp.FinalEffectiveDate.Date >= effectiveDateValue);
        }

        // Incluir relaciones
        if (includePhases)
        {
            Query.Include(qp => qp.Phases)
                 .ThenInclude(qpp => qpp.Phase);
        }

        // Ordenamiento por defecto
        Query.OrderBy(qp => qp.PlanName);
    }
}
