using ShopFinance.Application.Features.Sales.DTOs;

namespace ShopFinance.Application.Features.Sales.Queries;

public class GetSaleByIdQuery : IQuery<Result<SaleDetailDto>>
{
    public Guid SaleId { get; set; }
}
