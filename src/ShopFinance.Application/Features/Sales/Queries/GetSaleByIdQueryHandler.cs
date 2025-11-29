using ShopFinance.Application.Features.Sales.DTOs;
using ShopFinance.Domain.Specifications;

namespace ShopFinance.Application.Features.Sales.Queries;

internal class GetSaleByIdQueryHandler : IQueryHandler<GetSaleByIdQuery, Result<SaleDetailDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetSaleByIdQueryHandler> _logger;

    public GetSaleByIdQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetSaleByIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<SaleDetailDto>> HandleAsync(
        GetSaleByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var spec = new SaleByIdWithDetailsSpec(query.SaleId);
            var sale = await _unitOfWork.Sales.GetBySpecAsync(spec, cancellationToken);

            if (sale == null)
            {
                _logger.LogWarning("No se encontró la venta con ID: {SaleId}", query.SaleId);
                return Result.Error($"No se encontró la venta con ID: {query.SaleId}");
            }

            // Mapeo automático con Mapster
            var saleDto = sale.Adapt<SaleDetailDto>();

            _logger.LogDebug("Venta obtenida exitosamente - ID: {SaleId}, Número: {SaleNumber}",
                sale.Id, sale.SaleNumber);

            return Result.Success(saleDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener la venta con ID: {SaleId}", query.SaleId);
            return Result.Error($"Error al obtener la venta: {ex.Message}");
        }
    }
}