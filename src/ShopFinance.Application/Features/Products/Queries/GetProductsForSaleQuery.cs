using ShopFinance.Application.Features.Products.DTOs;
using ShopFinance.Domain.Specifications;

namespace ShopFinance.Application.Features.Products.Queries;

public class GetProductsForSaleQuery : IQuery<List<ProductForSaleDto>>
{
    public string? SearchText { get; set; }
    public int? CategoryId { get; set; }
    public bool? InStockOnly { get; set; } = true;
    public bool? IsActive { get; set; } = true;
}

internal class GetProductsForSaleQueryHandler : IQueryHandler<GetProductsForSaleQuery, List<ProductForSaleDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetProductsForSaleQueryHandler> _logger;

    public GetProductsForSaleQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetProductsForSaleQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<List<ProductForSaleDto>> HandleAsync(
        GetProductsForSaleQuery query,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Crear specification con los filtros
            var spec = new ProductsForSaleSpec(
                searchText: query.SearchText,
                categoryId: query.CategoryId,
                inStockOnly: query.InStockOnly,
                isActive: query.IsActive
            );

            // Obtener productos usando el nuevo método GetListAsync
            var products = await _unitOfWork.Products.GetListAsync(spec, cancellationToken);

            // Mapear a DTO
            var result = products.Select(p => new ProductForSaleDto
            {
                Id = p.Id,
                Name = p.Name,
                Code = p.Code,
                CodeSku = p.CodeSku,
                CostPrice = p.CostPrice,
                SalePrice = p.SalePrice,
                Stock = p.Stock,
                StockMin = p.StockMin,
                CategoryName = p.Category?.Name ?? "Sin categoría",
                State = p.State
            }).ToList();

            _logger.LogDebug(
                "Se obtuvieron {Count} productos para venta. Filtros: SearchText={SearchText}, CategoryId={CategoryId}, InStockOnly={InStockOnly}",
                result.Count, query.SearchText, query.CategoryId, query.InStockOnly);

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