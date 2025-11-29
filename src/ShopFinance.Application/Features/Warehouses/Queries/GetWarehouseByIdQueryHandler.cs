using ShopFinance.Application.Features.Warehouses.DTOs;

namespace ShopFinance.Application.Features.Warehouses.Queries;

public class GetWarehouseByIdQueryHandler : IQueryHandler<GetWarehouseByIdQuery, Result<WarehouseViewDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetWarehouseByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<WarehouseViewDto>> HandleAsync(GetWarehouseByIdQuery message, CancellationToken cancellationToken = default)
    {
        var warehouse = await _unitOfWork.Warehouses.GetByIdAsync(message.WarehouseId, cancellationToken); // Cambié "category" por "warehouse"
        if (warehouse == null)
        {
            return Result.NotFound("Warehouse not found");
        }
        var result = warehouse.Adapt<WarehouseViewDto>();
        return Result.Success(result);
    }
}