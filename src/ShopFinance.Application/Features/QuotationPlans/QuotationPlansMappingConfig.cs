using ShopFinance.Application.Features.QuotationPlans.DTOs;
using ShopFinance.Domain.Entities;

namespace ShopFinance.Application.Features.QuotationPlans;

public class QuotationPlansMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<QuotationPlan, QuotationPlanListItemDto>()
            .Map(o => o.PhaseCount, d => d.Phases.Count());
    }
}
