namespace ShopFinance.Application.Features.Warehouses.Commands;

internal class DeleteWarehouseCommandHandler : ICommandHandler<DeleteWarehouseCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteWarehouseCommand> _logger;

    public DeleteWarehouseCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteWarehouseCommand> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> HandleAsync(DeleteWarehouseCommand command, CancellationToken cancellationToken = default)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var warehouse = await _unitOfWork.Warehouses.GetByIdAsync(command.Id, cancellationToken);
            if (warehouse == null)
            {
                return Result.NotFound("Warehouse not found.");
            }

            // Verificar si hay productos asociados antes de eliminar
            var hasProducts = await _unitOfWork.WarehouseProducts.AnyAsync(wp => wp.WarehouseId == command.Id, cancellationToken);
            if (hasProducts)
            {
                return Result.Error("Cannot delete warehouse with associated products. Remove products first.");
            }

            // Verificar si hay movimientos de stock
            var hasStockMovements = await _unitOfWork.StockMovements.AnyAsync(sm => sm.WarehouseId == command.Id, cancellationToken);
            if (hasStockMovements)
            {
                return Result.Error("Cannot delete warehouse with stock movement history.");
            }

            await _unitOfWork.Warehouses.DeleteAsync(warehouse);
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Warehouse {WarehouseId} deleted successfully", command.Id);
            return Result.Success();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Error deleting warehouse {WarehouseId}", command.Id);
            return Result.Error($"Error deleting warehouse: {ex.Message}");
        }
    }
}