using ShopFinance.Application.Services.TaxRates.DTOs;
using ShopFinance.Domain.Entities;

namespace ShopFinance.Application.Services.TaxRates;

public class TaxRatesMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<TaxRate, TaxRateListItemDto>()
            .Map(dest => dest.Id, src => src.Id);

        config.NewConfig<TaxRate, TaxRateEditDto>()
            .Map(dest => dest.TaxRateId, src => src.Id);

        config.NewConfig<TaxRateEditDto, TaxRate>();
    }
}