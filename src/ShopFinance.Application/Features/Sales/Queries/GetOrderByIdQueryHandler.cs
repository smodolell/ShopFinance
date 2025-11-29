using ShopFinance.Application.Features.Sales.DTOs;
using ShopFinance.Domain.Specifications;

namespace ShopFinance.Application.Features.Sales.Queries;

internal class GetOrderByIdQueryHandler : IQueryHandler<GetOrderByIdQuery, Result<OrderViewDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetOrderByIdQueryHandler> _logger;

    public GetOrderByIdQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetOrderByIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<OrderViewDto>> HandleAsync(
        GetOrderByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var spec = new OrderByIdWithDetailsSpec(query.OrderId);
            var order = await _unitOfWork.Orders.GetBySpecAsync(spec, cancellationToken);

            if (order == null)
            {
                _logger.LogWarning("No se encontró la orden con ID: {OrderId}", query.OrderId);
                return Result.Error($"No se encontró la orden con ID: {query.OrderId}");
            }

            // Mapeo automático con Mapster
            var orderDto = order.Adapt<OrderViewDto>();

            _logger.LogDebug("Orden obtenida exitosamente - ID: {OrderId}, Número: {OrderNumber}",
                order.Id, order.OrderNumber);

            return Result.Success(orderDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener la orden con ID: {OrderId}", query.OrderId);
            return Result.Error($"Error al obtener la orden: {ex.Message}");
        }
    }
}