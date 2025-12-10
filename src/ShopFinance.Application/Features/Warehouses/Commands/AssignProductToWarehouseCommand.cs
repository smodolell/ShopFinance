using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Enums;

namespace ShopFinance.Application.Features.Warehouses.Commands;

public class AssignProductToWarehouseCommand : ICommand<Result<bool>>
{
    public Guid ProductId { get; set; }
    public Guid WarehouseId { get; set; }
    public int Stock { get; set; } = 0;
    public int StockMin { get; set; } = 0;
    public int StockMax { get; set; } = 100;
    public string? Location { get; set; }
}

internal class AssignProductToWarehouseCommandHandler : ICommandHandler<AssignProductToWarehouseCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AssignProductToWarehouseCommand> _logger;

    public AssignProductToWarehouseCommandHandler(IUnitOfWork unitOfWork, ILogger<AssignProductToWarehouseCommand> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<bool>> HandleAsync(AssignProductToWarehouseCommand command, CancellationToken cancellationToken = default)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            // Verificar que el producto existe
            var product = await _unitOfWork.Products.GetByIdAsync(command.ProductId, cancellationToken);
            if (product == null)
            {
                return Result.NotFound("Producto no encontrado");
            }

            // Verificar que el almacén existe
            var warehouse = await _unitOfWork.Warehouses.GetByIdAsync(command.WarehouseId, cancellationToken);
            if (warehouse == null)
            {
                return Result.NotFound("Almacén no encontrado");
            }

            // Verificar que no esté ya asignado
            var existingAssignment = await _unitOfWork.WarehouseProducts
                .AnyAsync(wp => wp.WarehouseId == command.WarehouseId && wp.ProductId == command.ProductId, cancellationToken);

            if (existingAssignment)
            {
                return Result.Error("El producto ya está asignado a este almacén");
            }

            // Crear la asignación
            var warehouseProduct = new WarehouseProduct
            {
                Id = Guid.NewGuid(),
                WarehouseId = command.WarehouseId,
                ProductId = command.ProductId,
                StockQuantity = command.Stock,
                StockMin = command.StockMin,
                StockMax = command.StockMax,
                Location = command.Location,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.WarehouseProducts.AddAsync(warehouseProduct);

            // Registrar movimiento de stock si hay stock inicial
            if (command.Stock > 0)
            {
                var stockMovement = new StockMovement
                {
                    Id = Guid.NewGuid(),
                    WarehouseId = command.WarehouseId,
                    ProductId = command.ProductId,
                    MovementType = MovementType.Entry,
                    Quantity = command.Stock,
                    PreviousStock = 0,
                    NewStock = command.Stock,
                    Notes = "Asignación inicial",
                    CreatedAt = DateTime.UtcNow,
                    MovementDate = DateTime.UtcNow,
                    Source = MovementSource.Manual
                };

                await _unitOfWork.StockMovements.AddAsync(stockMovement);
            }

            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Producto {ProductId} asignado al almacén {WarehouseId}", command.ProductId, command.WarehouseId);
            return Result.Success(true);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Error al asignar producto al almacén");
            return Result.Error($"Error al asignar producto: {ex.Message}");
        }
    }
}