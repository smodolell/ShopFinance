using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Enums;
using ShopFinance.Domain.Specifications;

namespace ShopFinance.Application.Features.Sales.Commands;

internal class ConfirmOrderCommandHandler : ICommandHandler<ConfirmOrderCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<ConfirmOrderCommand> _validator;
    private readonly ILogger<ConfirmOrderCommandHandler> _logger;

    public ConfirmOrderCommandHandler(
        IUnitOfWork unitOfWork,
        IValidator<ConfirmOrderCommand> validator,
        ILogger<ConfirmOrderCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid>> HandleAsync(ConfirmOrderCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Result.Invalid(validationResult.AsErrors());
        }

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            // 1. Obtener y validar la orden
            var order = await _unitOfWork.Orders.GetBySpecAsync(
                new OrderByIdWithDetailsSpec(command.OrderId), cancellationToken);

            if (order == null)
                return Result.Error($"No se encontró la orden con ID {command.OrderId}");

            if (order.Status != OrderStatus.Draft)
                return Result.Error("Solo se pueden confirmar órdenes en estado Borrador");

            if (order.Items.Count == 0)
                return Result.Error("La orden no tiene items para confirmar");

            // 2. Validar stock de productos
            foreach (var item in order.Items)
            {
                if (item.Product.Stock < item.Quantity)
                {
                    return Result.Error($"Stock insuficiente para {item.Product.Name}. Disponible: {item.Product.Stock}, Solicitado: {item.Quantity}");
                }
            }

            // 3. Crear la venta
            var sale = new Sale
            {
                Id = Guid.NewGuid(),
                SaleNumber = await GenerateSaleNumber(cancellationToken),
                OrderId = order.Id,
                SaleDate = DateTime.UtcNow,
                Status = SaleStatus.Completed,
                InvoiceNumber = command.InvoiceNumber,
                PaymentMethod = command.PaymentMethod,
                TotalAmount = order.TotalAmount
            };

            // 4. Crear items de venta
            foreach (var orderItem in order.Items)
            {
                var saleItem = new SaleItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = orderItem.ProductId,
                    Quantity = orderItem.Quantity,
                    CostPrice = orderItem.Product.CostPrice, // Guardar costo histórico
                    UnitPrice = orderItem.UnitPrice
                };
                sale.Items.Add(saleItem);

                // 5. Actualizar stock del producto
                orderItem.Product.Stock -= orderItem.Quantity;
                await _unitOfWork.Products.UpdateAsync(orderItem.Product);
            }

            // 6. Cambiar estado de la orden
            order.Status = OrderStatus.Confirmed;

            // 7. Guardar cambios
            await _unitOfWork.Sales.AddAsync(sale);
            await _unitOfWork.Orders.UpdateAsync(order);
            await _unitOfWork.CommitAsync();

            _logger.LogInformation(
                "Orden confirmada y venta creada - Order: {OrderId}, Sale: {SaleId}",
                order.Id, sale.Id);

            return Result.Success(sale.Id, "Orden confirmada y venta generada exitosamente");
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Error al confirmar orden {OrderId}", command.OrderId);
            return Result.Error($"Error al confirmar la orden: {ex.Message}");
        }
    }

    private async Task<string> GenerateSaleNumber(CancellationToken cancellationToken)
    {
        var today = DateTime.Today;
        var spec = new SalesFromTodaySpec(today);
        var salesToday = await _unitOfWork.Sales
            .CountAsync(spec, cancellationToken);

        var sequence = salesToday + 1;
        return $"SALE-{today:yyyyMMdd}-{sequence:000}";
    }
}