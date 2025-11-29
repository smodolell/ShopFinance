using Ardalis.Result;
using Microsoft.Extensions.Logging;
using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Enums;
using ShopFinance.Domain.Services.Dtos;
using ShopFinance.Domain.Specifications;

namespace ShopFinance.Domain.Services;

public interface IStockService
{
    Task<Result> AdjustStockAsync(Guid warehouseId, Guid productId, int newQuantity, string reason, string? notes = null);
    Task<Result> TransferStockAsync(Guid fromWarehouseId, Guid toWarehouseId, List<ProductQuantity> items, string? notes = null);
    Task<Result> ProcessSaleAsync(Guid warehouseId, List<SaleItem> items);
    //Task<Result> ProcessPurchaseAsync(Guid warehouseId, List<PurchaseItem> items);
    //Task<Result> RegisterStockCountAsync(Guid warehouseId, List<StockCountItem> items, string? notes = null);
    Task<List<StockAlert>> GetStockAlertsAsync(Guid warehouseId);
    Task<WarehouseSummary> GetWarehouseSummaryAsync(Guid warehouseId);
}


public class StockService : IStockService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<StockService> _logger;

    public StockService(IUnitOfWork unitOfWork, ILogger<StockService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    public async Task<Result> AdjustStockAsync(Guid warehouseId, Guid productId, int newQuantity, string reason, string? notes = null)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            // 1. Obtener el producto en el almacén
            var warehouseProduct = await _unitOfWork.WarehouseProducts
                .GetBySpecAsync(new WarehouseProductSpec(warehouseId, productId));

            if (warehouseProduct == null)
                return Result.Error("Producto no encontrado en el almacén especificado");

            var previousStock = warehouseProduct.StockQuantity;

            // 2. Crear movimiento de stock
            var movement = new StockMovement
            {
                Id = Guid.NewGuid(),
                WarehouseId = warehouseId,
                ProductId = productId,
                MovementType = MovementType.Adjustment,
                Source = MovementSource.Manual,
                Quantity = newQuantity - previousStock,
                PreviousStock = previousStock,
                NewStock = newQuantity,
                Notes = $"{reason}. {notes}",
                MovementDate = DateTime.UtcNow
            };

            // 3. Actualizar stock
            warehouseProduct.StockQuantity = newQuantity;
            warehouseProduct.UpdatedAt = DateTime.UtcNow;

            // 4. Guardar cambios
            await _unitOfWork.StockMovements.AddAsync(movement);
            await _unitOfWork.WarehouseProducts.UpdateAsync(warehouseProduct);

            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Stock ajustado - Producto: {ProductId}, Almacén: {WarehouseId}, Cantidad: {NewQuantity}",
                productId, warehouseId, newQuantity);

            return Result.Success();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Error al ajustar stock - Producto: {ProductId}, Almacén: {WarehouseId}",
                productId, warehouseId);
            return Result.Error($"Error al ajustar stock: {ex.Message}");
        }
    }


    public async Task<List<StockAlert>> GetStockAlertsAsync(Guid warehouseId)
    {
        var alerts = new List<StockAlert>();
        var spec = new WarehouseProductsWithLowStockSpec(warehouseId);
        var lowStockProducts = await _unitOfWork.WarehouseProducts.GetListAsync(spec);

        foreach (var wp in lowStockProducts)
        {
            alerts.Add(new StockAlert
            {
                Id = Guid.NewGuid(),
                ProductId = wp.ProductId,
                WarehouseId = wp.WarehouseId,
                AlertType = wp.StockQuantity == 0 ? StockAlertType.OutOfStock : StockAlertType.LowStock,
                CurrentStock = wp.StockQuantity,
                Threshold = wp.StockMin,
                Message = wp.StockQuantity == 0 ?
                    $"Producto {wp.Product.Name} sin stock" :
                    $"Stock bajo para {wp.Product.Name}. Actual: {wp.StockQuantity}, Mínimo: {wp.StockMin}",
                AlertDate = DateTime.UtcNow
            });
        }

        return alerts;
    }


    public Task<WarehouseSummary> GetWarehouseSummaryAsync(Guid warehouseId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> ProcessSaleAsync(Guid warehouseId, List<SaleItem> items)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            foreach (var saleItem in items)
            {
                var warehouseProduct = await _unitOfWork.WarehouseProducts
                    .GetBySpecAsync(new WarehouseProductSpec(warehouseId, saleItem.ProductId));

                if (warehouseProduct == null)
                    return Result.Error($"Producto {saleItem.ProductId} no disponible en el almacén");

                if (warehouseProduct.StockQuantity < saleItem.Quantity)
                    return Result.Error($"Stock insuficiente para {warehouseProduct.Product.Name}");

                var previousStock = warehouseProduct.StockQuantity;
                var newStock = previousStock - saleItem.Quantity;

                // Crear movimiento de salida
                var movement = new StockMovement
                {
                    Id = Guid.NewGuid(),
                    WarehouseId = warehouseId,
                    ProductId = saleItem.ProductId,
                    ReferenceId = saleItem.SaleId, // Relacionar con la venta
                    MovementType = MovementType.Exit,
                    Source = MovementSource.Sale,
                    Quantity = saleItem.Quantity,
                    PreviousStock = previousStock,
                    NewStock = newStock,
                    Notes = $"Venta #{saleItem.SaleId}",
                    MovementDate = DateTime.UtcNow
                };

                // Actualizar stock
                warehouseProduct.StockQuantity = newStock;
                warehouseProduct.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.StockMovements.AddAsync(movement);
                await _unitOfWork.WarehouseProducts.UpdateAsync(warehouseProduct);
            }

            await _unitOfWork.CommitAsync();
            return Result.Success();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Error al procesar stock de venta");
            return Result.Error($"Error al procesar stock de venta: {ex.Message}");
        }
    }


    public async Task<Result> TransferStockAsync(Guid fromWarehouseId, Guid toWarehouseId, List<ProductQuantity> items, string? notes = null)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            // 1. Crear registro de transferencia
            var transfer = new StockTransfer
            {
                Id = Guid.NewGuid(),
                TransferNumber = await GenerateTransferNumber(),
                FromWarehouseId = fromWarehouseId,
                ToWarehouseId = toWarehouseId,
                Status = TransferStatus.Pending,
                TransferDate = DateTime.UtcNow,
                Notes = notes
            };

            // 2. Procesar cada item
            foreach (var item in items)
            {
                // Validar stock disponible en almacén origen
                var fromWarehouseProduct = await _unitOfWork.WarehouseProducts
                    .GetBySpecAsync(new WarehouseProductSpec(fromWarehouseId, item.ProductId));

                if (fromWarehouseProduct == null || fromWarehouseProduct.StockQuantity < item.Quantity)
                    return Result.Error($"Stock insuficiente para el producto {item.ProductId} en el almacén origen");

                // Crear item de transferencia
                var transferItem = new StockTransferItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    QuantitySent = 0,
                    QuantityReceived = 0
                };
                transfer.Items.Add(transferItem);
            }

            await _unitOfWork.StockTransfers.AddAsync(transfer);
            await _unitOfWork.CommitAsync();

            return Result.Success();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Error al crear transferencia de stock");
            return Result.Error($"Error al crear transferencia: {ex.Message}");
        }
    }
    private async Task<string> GenerateTransferNumber()
    {
        var today = DateTime.Today;
        var spec = new StockTransfersTodaySpec(today);
        var transfersToday = await _unitOfWork.StockTransfers.CountAsync(spec);
        return $"TRF-{today:yyyyMMdd}-{transfersToday + 1:000}";
    }
}