using ShopFinance.Application.Features.Sales.DTOs;
using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Enums;
using ShopFinance.Domain.Specifications;

namespace ShopFinance.Application.Features.Sales.Commands;

internal class CreateOrUpdateOrderCommandHandler : ICommandHandler<CreateOrUpdateOrderCommand, Result<OrderResultDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateOrUpdateOrderCommand> _validator;
    private readonly ILogger<CreateOrUpdateOrderCommandHandler> _logger;

    public CreateOrUpdateOrderCommandHandler(
        IUnitOfWork unitOfWork,
        IValidator<CreateOrUpdateOrderCommand> validator,
        ILogger<CreateOrUpdateOrderCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<OrderResultDto>> HandleAsync(
        CreateOrUpdateOrderCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Result.Invalid(validationResult.AsErrors());
        }

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            Order order;
            bool isNewOrder = !command.OrderId.HasValue;

            if (isNewOrder)
            {
                // CREAR NUEVA ORDEN
                order = await CreateNewOrder(command, cancellationToken);
            }
            else
            {
                // ACTUALIZAR ORDEN EXISTENTE
                order = await UpdateExistingOrder(command, cancellationToken);
            }

            // CONFIRMAR ORDEN SI ES REQUERIDO
            Guid? saleId = null;
            if (command.ConfirmOrder)
            {
                saleId = await ConfirmOrderAndCreateSale(order, command, cancellationToken);
            }

            await _unitOfWork.CommitAsync();

            var result = new OrderResultDto
            {
                OrderId = order.Id,
                SaleId = saleId,
                OrderNumber = order.OrderNumber,
                Status = order.Status
            };

            _logger.LogInformation(
                "Orden {(Action)} exitosamente - ID: {OrderId}, Confirmada: {Confirmed}",
                isNewOrder ? "creada" : "actualizada", order.Id, command.ConfirmOrder);

            return Result.Success(result, GetSuccessMessage(isNewOrder, command.ConfirmOrder));
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Error al procesar orden");
            return Result.Error($"Error al procesar la orden: {ex.Message}");
        }
    }

    private async Task<Order> CreateNewOrder(CreateOrUpdateOrderCommand command, CancellationToken cancellationToken)
    {
        var orderNumber = await GenerateOrderNumber(cancellationToken);

        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = command.CustomerId,
            OrderNumber = orderNumber,
            OrderDate = DateTime.UtcNow,
            Status = command.ConfirmOrder ? OrderStatus.Confirmed : OrderStatus.Draft,
            Notes = command.Notes,
            RequiredDate = command.RequiredDate,
            TotalAmount = command.Items.Sum(x => x.Quantity * x.UnitPrice)
        };

        // Agregar items
        foreach (var itemDto in command.Items)
        {
            var orderItem = new OrderItem
            {
                Id = Guid.NewGuid(),
                ProductId = itemDto.ProductId,
                Quantity = itemDto.Quantity,
                UnitPrice = itemDto.UnitPrice
            };
            order.Items.Add(orderItem);
        }

        await _unitOfWork.Orders.AddAsync(order);
        return order;
    }

    private async Task<Order> UpdateExistingOrder(CreateOrUpdateOrderCommand command, CancellationToken cancellationToken)
    {
        if (command.OrderId == null)
            throw new Exception("El ID de la orden es requerido para actualizar");

        var order = await _unitOfWork.Orders.GetBySpecAsync(
            new OrderByIdWithDetailsSpec(command.OrderId.Value), cancellationToken);

        if (order == null)
            throw new Exception($"No se encontró la orden con ID {command.OrderId}");

        if (order.Status != OrderStatus.Draft)
            throw new Exception("Solo se pueden modificar órdenes en estado Borrador");

        // Actualizar propiedades
        order.CustomerId = command.CustomerId;
        order.Notes = command.Notes;
        order.RequiredDate = command.RequiredDate;
        order.TotalAmount = command.Items.Sum(x => x.Quantity * x.UnitPrice);
        order.UpdatedAt = DateTime.UtcNow;

        if (command.ConfirmOrder)
        {
            order.Status = OrderStatus.Confirmed;
        }

        // Actualizar items
        await UpdateOrderItems(order, command.Items, cancellationToken);

        await _unitOfWork.Orders.UpdateAsync(order);
        return order;
    }

    private async Task<Guid?> ConfirmOrderAndCreateSale(Order order, CreateOrUpdateOrderCommand command, CancellationToken cancellationToken)
    {
        // Validar stock
        foreach (var item in order.Items)
        {
            if (item.Product.Stock < item.Quantity)
            {
                throw new Exception(
                    $"Stock insuficiente para {item.Product.Name}. Disponible: {item.Product.Stock}, Solicitado: {item.Quantity}");
            }
        }

        // Crear venta
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

        // Crear items de venta y actualizar stock
        foreach (var orderItem in order.Items)
        {
            var saleItem = new SaleItem
            {
                Id = Guid.NewGuid(),
                ProductId = orderItem.ProductId,
                Quantity = orderItem.Quantity,
                CostPrice = orderItem.Product.CostPrice,
                UnitPrice = orderItem.UnitPrice
            };
            sale.Items.Add(saleItem);

            // Actualizar stock
            orderItem.Product.Stock -= orderItem.Quantity;
            await _unitOfWork.Products.UpdateAsync(orderItem.Product);
        }

        await _unitOfWork.Sales.AddAsync(sale);
        return sale.Id;
    }

    private async Task UpdateOrderItems(Order order, List<OrderItemDto> newItems, CancellationToken cancellationToken)
    {
        // Eliminar items existentes
        var existingItems = order.Items.ToList();

        order.Items.Clear();

        foreach (var existingItem in existingItems)
        {
            
            await _unitOfWork.OrderItems.DeleteAsync(existingItem.Id);
        }

        // Agregar nuevos items
        foreach (var itemDto in newItems)
        {
            var orderItem = new OrderItem
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                ProductId = itemDto.ProductId,
                Quantity = itemDto.Quantity,
                UnitPrice = itemDto.UnitPrice
            };
            await _unitOfWork.OrderItems.AddAsync(orderItem);
        }
    }

    private async Task<string> GenerateOrderNumber(CancellationToken cancellationToken)
    {
        var today = DateTime.Today;
        var spec = new OrdersFromTodaySpec(today);
        var ordersToday = await _unitOfWork.Orders
            .CountAsync(spec, cancellationToken);
        return $"ORD-{today:yyyyMMdd}-{ordersToday + 1:000}";
    }

    private async Task<string> GenerateSaleNumber(CancellationToken cancellationToken)
    {
        var today = DateTime.Today;
        var spec = new SalesFromTodaySpec(today);
        var salesToday = await _unitOfWork.Sales
            .CountAsync(spec, cancellationToken);
        return $"SALE-{today:yyyyMMdd}-{salesToday + 1:000}";
    }

    private string GetSuccessMessage(bool isNewOrder, bool isConfirmed)
    {
        if (isNewOrder)
        {
            return isConfirmed ? "Orden creada y confirmada exitosamente" : "Orden creada exitosamente";
        }
        else
        {
            return isConfirmed ? "Orden actualizada y confirmada exitosamente" : "Orden actualizada exitosamente";
        }
    }
}