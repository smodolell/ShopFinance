using ShopFinance.Domain.Entities;

namespace ShopFinance.Domain.Specifications;

public class WarehouseProductForSaleSpec : Specification<WarehouseProduct>
{
    public WarehouseProductForSaleSpec(string? searchText, int? categoryId, List<Guid> warehouseIds)
    {

        Query.Where(r => r.Product.State == Enums.ProductState.Active);
        Query.Where(r => r.StockQuantity > 0);

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            searchText = searchText.ToLower();
            Query.Where(
                u => u.Product.Code.ToLower().Contains(searchText) ||
                u.Product.CodeSku.ToLower().Contains(searchText) ||
                u.Product.Name.ToLower().Contains(searchText) ||
                u.Product.Description.ToLower().Contains(searchText)
            );
        }
        if (categoryId.HasValue)
        {
            Query.Where(u => u.Product.CategoryId == categoryId.Value);
        }
        if (warehouseIds.Any())
        {
            Query.Where(u => warehouseIds.Contains(u.WarehouseId));
        }

        Query.Include(r => r.Product);
    }
}

