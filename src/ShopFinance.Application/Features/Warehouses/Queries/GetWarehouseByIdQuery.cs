using ShopFinance.Application.Features.Warehouses.DTOs;

namespace ShopFinance.Application.Features.Warehouses.Queries;

public class GetWarehouseByIdQuery : IQuery<Result<WarehouseViewDto>>
{
    public Guid WarehouseId { get; set; }

   
}
