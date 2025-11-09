using ShopFinance.Application.Features.Customers.DTOs;

namespace ShopFinance.Application.Features.Customers.Queries;

public class GetCustomerByIdQuery : IQuery<Result<CustomerViewDto>>
{
    public Guid CustomerId { get; set; }
}
