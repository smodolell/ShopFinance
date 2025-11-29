using Ardalis.Result;
using ShopFinance.Application.Features.Sales.DTOs;

namespace ShopFinance.Application.Features.Sales.Queries;

public class GetOrderByIdQuery:IQuery<Result<OrderViewDto>>
{
    public Guid OrderId { get; set; }
}
