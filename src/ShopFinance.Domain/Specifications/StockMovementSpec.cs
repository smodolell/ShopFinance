using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Enums;

namespace ShopFinance.Domain.Specifications;

public class StockMovementSpec : Specification<StockMovement>
{
    public StockMovementSpec(
        Guid? warehouseId = null,
        Guid? productId = null,
        MovementType? movementType = null,
        MovementSource? source = null,
        DateTime? movementDateFrom = null,
        DateTime? movementDateTo = null,
        string? searchText = null)
    {
        // Filtro por almacén
        if (warehouseId.HasValue)
        {
            Query.Where(sm => sm.WarehouseId == warehouseId.Value);
        }

        // Filtro por producto
        if (productId.HasValue)
        {
            Query.Where(sm => sm.ProductId == productId.Value);
        }

        // Filtro por tipo de movimiento
        if (movementType.HasValue)
        {
            Query.Where(sm => sm.MovementType == movementType.Value);
        }

        // Filtro por fuente
        if (source.HasValue)
        {
            Query.Where(sm => sm.Source == source.Value);
        }

        // Filtro por rango de fechas
        if (movementDateFrom.HasValue)
        {
            Query.Where(sm => sm.MovementDate >= movementDateFrom.Value);
        }

        if (movementDateTo.HasValue)
        {
            Query.Where(sm => sm.MovementDate <= movementDateTo.Value.AddDays(1).AddSeconds(-1));
        }

        // Búsqueda por texto
        if (!string.IsNullOrEmpty(searchText))
        {
            Query.Where(sm =>
                sm.Product.Name.Contains(searchText) ||
                sm.Product.Code.Contains(searchText) ||
                sm.Warehouse.Name.Contains(searchText) ||
                sm.Notes != null && sm.Notes.Contains(searchText)
            );
        }
        Query.Include(sm => sm.Warehouse);
        Query.Include(sm => sm.Product);
    }
}