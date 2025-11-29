using ShopFinance.Domain.Enums;
namespace ShopFinance.Application.Features.Sales.Commands;

internal class DeleteOrderCommandHandler : ICommandHandler<DeleteOrderCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteOrderCommandHandler> _logger;

    public DeleteOrderCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteOrderCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> HandleAsync(DeleteOrderCommand command, CancellationToken cancellationToken = default)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(command.OrderId, cancellationToken);
            if (order == null)
            {
                return Result.Error($"No se encontró la orden con ID {command.OrderId}");
            }

            // Validar que solo se puedan eliminar órdenes en estado Draft
            if (order.Status != OrderStatus.Draft)
            {
                return Result.Error("Solo se pueden eliminar órdenes en estado Borrador (Draft)");
            }

            // Eliminar items primero (por la relación foreign key)
            var orderItems = order.Items.ToList();
            foreach (var item in orderItems)
            {
                await _unitOfWork.OrderItems.DeleteAsync(item);
            }

            // Eliminar la orden
            await _unitOfWork.Orders.DeleteAsync(order);
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Orden eliminada exitosamente - ID: {OrderId}, Número: {OrderNumber}",
                order.Id, order.OrderNumber);

            return Result.SuccessWithMessage("Orden eliminada exitosamente");
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Error al eliminar la orden con ID: {OrderId}", command.OrderId);
            return Result.Error($"Error al eliminar la orden: {ex.Message}");
        }
    }
}
