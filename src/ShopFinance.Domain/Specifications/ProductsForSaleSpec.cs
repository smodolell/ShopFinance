using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Enums;

namespace ShopFinance.Domain.Specifications;

public class ProductsForSaleSpec : Specification<Product>
{
    public ProductsForSaleSpec(
        string? searchText = null,
        int? categoryId = null,
        bool? inStockOnly = true,
        bool? isActive = true)
    {
        if (!string.IsNullOrWhiteSpace(searchText))
        {
            Query.Where(p =>
            p.Name.Contains(searchText) ||
            p.Code.Contains(searchText) ||
            (p.CodeSku != null && p.CodeSku.Contains(searchText)));
        }

        // Filtro por categoría
        if (categoryId.HasValue)
        {
            Query.Where(p => p.CategoryId == categoryId.Value);
        }

        // Filtro por stock
        if (inStockOnly == true)
        {
            Query.Where(p => p.Stock > 0);
        }

        // Filtro por estado activo
        if (isActive == true)
        {
            Query.Where(p => p.State == ProductState.Active);
        }

        // Incluir la categoría
        Query.Include(p => p.Category);

        // Ordenar por nombre
        Query.OrderBy(p => p.Name);
    }
}