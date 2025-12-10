using ShopFinance.Application.Services.Frequencies.DTOs;
using ShopFinance.Domain.Entities;

namespace ShopFinance.Application.Services.Frequencies;

public class FrequenciesMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Frequency, FrequencyListItemDto>()
            .Map(dest => dest.Id, src => src.Id);

        config.NewConfig<Frequency, FrequencyEditDto>()
            .Map(dest => dest.FrequencyId, src => src.Id);

        config.NewConfig<FrequencyEditDto, Frequency>();
    }
}