using ShopFinance.Application.Features.Customers.DTOs;
using ShopFinance.Domain.Entities;

namespace ShopFinance.Application.Features.Customers;

public class CustomersMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Customer, CustomerListItemDto>();
        config.NewConfig<Customer, CustomerViewDto>()
            .Map(o => o.CustomerId, d => d.Id);

        config.NewConfig<Customer, CustomerSearchDto>()
        .Map(o => o.CustomerId, d => d.Id);
    }
}
