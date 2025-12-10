using ShopFinance.Application.Services.PaymentTerms.DTOs;
using ShopFinance.Domain.Entities;

namespace ShopFinance.Application.Services.PaymentTerms;

public class PaymentTermsMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<PaymentTerm, PaymentTermListItemDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Ignore(dest => dest.ApproximateDays); // Propiedad calculada

        config.NewConfig<PaymentTerm, PaymentTermEditDto>()
            .Map(dest => dest.PaymentTermId, src => src.Id)
            .Ignore(dest => dest.ApproximateDays); // Propiedad calculada

        config.NewConfig<PaymentTermEditDto, PaymentTerm>()
            .Ignore(dest => dest.QuotationPlanPaymentTerms); // Ignorar relación
    }
}