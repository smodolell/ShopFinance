using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Enums;

namespace ShopFinance.Application.Features.Products.Commands;

public class CreateProductWithWarehouseCommand : ICommand<Result<Guid>>
{
    public int? CategoryId { get; set; }
    public ProductState? State { get; set; }
    public string Code { get; set; } = string.Empty;
    public string CodeSku { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal CostPrice { get; set; }
    public decimal SalePrice { get; set; }
    public int Stock { get; set; }
    public int StockMin { get; set; }
    public int StockMax { get; set; }

    public Guid WarehouseId { get; set; }
    public string? Location { get; set; }
}

public class CreateProductWithWarehouseCommandValidator : AbstractValidator<CreateProductWithWarehouseCommand>
{
    public CreateProductWithWarehouseCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Code).NotEmpty().MaximumLength(50);
        RuleFor(x => x.CostPrice).GreaterThanOrEqualTo(0);
        RuleFor(x => x.SalePrice).GreaterThanOrEqualTo(0);
        RuleFor(x => x.WarehouseId).NotEmpty();
        RuleFor(x => x.Stock).GreaterThanOrEqualTo(0);
        RuleFor(x => x.StockMin).GreaterThanOrEqualTo(0);
        RuleFor(x => x.StockMax).GreaterThan(x => x.StockMin);
    }
}

internal class CreateProductWithWarehouseCommandHandler : ICommandHandler<CreateProductWithWarehouseCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateProductWithWarehouseCommand> _logger;

    public CreateProductWithWarehouseCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateProductWithWarehouseCommand> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid>> HandleAsync(CreateProductWithWarehouseCommand command, CancellationToken cancellationToken = default)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            
            // 1. Verificar que el almacén existe
            var warehouse = await _unitOfWork.Warehouses.GetByIdAsync(command.WarehouseId, cancellationToken);
            if (warehouse == null)
            {
                return Result.NotFound("Almacén no encontrado");
            }

            // 2. Crear el producto
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = command.Name,
                Code = command.Code,
                CodeSku= command.CodeSku,
                Description = command.Description,
                CostPrice = command.CostPrice,
                SalePrice = command.SalePrice,
                CategoryId = command.CategoryId ?? 0,
                State = ProductState.Active,
                CreatedAt = DateTime.UtcNow,
                Stock = command.Stock,
                StockMin = command.StockMin,
                
            };

            await _unitOfWork.Products.AddAsync(product);

            // 3. Asignar producto al almacén
            var warehouseProduct = new WarehouseProduct
            {
                Id = Guid.NewGuid(),
                WarehouseId = warehouse.Id,
                ProductId = product.Id,
                StockQuantity = command.Stock,
                StockMin = command.StockMin,
                StockMax = command.StockMax,
                Location = command.Location,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.WarehouseProducts.AddAsync(warehouseProduct);

            // 4. Registrar movimiento de stock inicial si hay stock
            if (command.Stock > 0)
            {
                var stockMovement = new StockMovement
                {
                    Id = Guid.NewGuid(),
                    WarehouseId = warehouse.Id,
                    ProductId = product.Id,
                    MovementType = MovementType.Entry,
                    Quantity = command.Stock,
                    PreviousStock = 0,
                    NewStock = command.Stock,
                    Notes = "Stock inicial",
                    CreatedAt = DateTime.UtcNow,
                    MovementDate = DateTime.UtcNow,
                    Source = MovementSource.Manual
                };

                await _unitOfWork.StockMovements.AddAsync(stockMovement);
            }

            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Producto {ProductId} creado y asignado al almacén {WarehouseId}", product.Id, command.WarehouseId);
            return Result.Success(product.Id);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Error al crear producto con asignación a almacén");
            return Result.Error($"Error al crear producto: {ex.Message}");
        }
    }
}
