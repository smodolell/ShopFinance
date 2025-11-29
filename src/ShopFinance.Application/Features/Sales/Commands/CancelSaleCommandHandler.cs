using ShopFinance.Domain.Enums;
using ShopFinance.Domain.Specifications;

namespace ShopFinance.Application.Features.Sales.Commands;

internal class CancelSaleCommandHandler : ICommandHandler<CancelSaleCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CancelSaleCommandHandler> _logger;

    public CancelSaleCommandHandler(IUnitOfWork unitOfWork, ILogger<CancelSaleCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> HandleAsync(CancelSaleCommand command, CancellationToken cancellationToken = default)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var sale = await _unitOfWork.Sales.GetBySpecAsync(
                new SaleByIdWithDetailsSpec(command.SaleId), cancellationToken);

            if (sale == null)
                return Result.Error($"No se encontró la venta con ID {command.SaleId}");

            if (sale.Status != SaleStatus.Completed)
                return Result.Error("Solo se pueden cancelar ventas completadas");

            // Revertir stock de productos
            foreach (var saleItem in sale.Items)
            {
                if (saleItem.Product != null)
                {
                    saleItem.Product.Stock += saleItem.Quantity;
                    await _unitOfWork.Products.UpdateAsync(saleItem.Product);
                }
            }

            // Cambiar estado de la venta
            sale.Status = SaleStatus.Cancelled;
            await _unitOfWork.Sales.UpdateAsync(sale);

            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Venta cancelada exitosamente - ID: {SaleId}", sale.Id);
            return Result.SuccessWithMessage("Venta cancelada exitosamente");
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Error al cancelar la venta con ID: {SaleId}", command.SaleId);
            return Result.Error($"Error al cancelar la venta: {ex.Message}");
        }
    }
}

