using ShopFinance.Application.Services.InterestRates.DTOs;
using ShopFinance.Domain.Entities;

namespace ShopFinance.Application.Services.InterestRates;
public class InterestRatesMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<InterestRate, InterestRateListItemDto>()
            .Map(dest => dest.Id, src => src.Id);

        config.NewConfig<InterestRate, InterestRateEditDto>()
           .Map(dest => dest.InterestRateId, src => src.Id);

        config.NewConfig<InterestRateEditDto, InterestRate>();

    }
}