using ShopFinance.Application.Features.Products.DTOs;
using ShopFinance.Domain.Specifications;

namespace ShopFinance.Application.Features.Products.Queries;

public class GetProductsSearchQuery : IQuery<List<ProductSearchDto>>
{
    public string? SearchText { get; set; }
    public int? CategoryId { get; set; }
}

internal class GetProductsSearchQueryHandler : IQueryHandler<GetProductsSearchQuery, List<ProductSearchDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetProductsForSaleQueryHandler> _logger;

    public GetProductsSearchQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetProductsForSaleQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<List<ProductSearchDto>> HandleAsync(
        GetProductsSearchQuery query,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Crear specification con los filtros
            var spec = new ProductSpec(
                query.SearchText,
                null,
                query.CategoryId
            );

            // Obtener productos usando el nuevo método GetListAsync
            var products = await _unitOfWork.Products.GetListAsync(spec, cancellationToken);

            // Mapear a DTO
            var result = products.Select(p => new ProductSearchDto
            {
                Id = p.Id,
                Name = p.Name,
                Code = p.Code,
                CodeSku = p.CodeSku,
            }).ToList();

            _logger.LogDebug(
                "Se obtuvieron {Count} productos para venta. Filtros: SearchText={SearchText}, CategoryId={CategoryId}",
                result.Count, query.SearchText, query.CategoryId);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error al obtener productos para venta. Filtros: SearchText={SearchText}, CategoryId={CategoryId}",
                query.SearchText, query.CategoryId);

            throw; // O retornar una lista vacía dependiendo de tu política de errores
        }
    }
}
