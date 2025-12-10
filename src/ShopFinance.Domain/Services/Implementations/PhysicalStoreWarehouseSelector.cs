using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Enums;
using ShopFinance.Domain.Services.Dtos;
using ShopFinance.Domain.Specifications;
using Microsoft.Extensions.Logging;

namespace ShopFinance.Domain.Services.Implementations;

public class PhysicalStoreWarehouseSelector : IWarehouseSelectorService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<PhysicalStoreWarehouseSelector> _logger;

    public PhysicalStoreWarehouseSelector(
        IUnitOfWork unitOfWork,
        ILogger<PhysicalStoreWarehouseSelector> logger)
    {
        this._unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<WarehouseSelectionResult> SelectWarehouseAsync(
        List<ProductStockRequest> products,
        SaleContext context)
    {
        _logger.LogInformation(
            "Selección almacén para venta física. Tienda: {StoreId}",
            context.StoreId);

        if (!context.StoreId.HasValue)
            return CreateErrorResult("StoreId requerido para ventas físicas");

        // Obtener almacenes de la tienda (Physical type)
        var storeWarehouses = await GetStoreWarehouses(context.StoreId.Value);

        if (!storeWarehouses.Any())
            return CreateErrorResult($"No hay almacenes para la tienda {context.StoreId}");

        // Procesar productos
        var allocations = new List<WarehouseAllocation>();
        var errors = new List<string>();

        foreach (var productRequest in products)
        {
            var allocationResult = await AllocateProduct(
                productRequest,
                storeWarehouses);

            if (allocationResult.Success)
                allocations.Add(allocationResult.Allocation!);
            else
                errors.Add(allocationResult.ErrorMessage!);
        }

        if (errors.Count > 0)
            return CreateErrorResult($"Errores: {string.Join("; ", errors)}");

        return CreateSuccessResult(allocations);
    }

    /// <summary>
    /// Obtiene almacenes de UNA tienda específica
    /// Estrategia: Buscar por código o nombre que contenga el StoreId
    /// </summary>
    private async Task<List<Warehouse>> GetStoreWarehouses(Guid storeId)
    {
        try
        {
            // 1. Buscar almacén con código específico de tienda
            var storeCodeSpec = new WarehouseByCodeSpec($"STORE-{storeId}");
            var storeByCode = await _unitOfWork.Warehouses.GetBySpecAsync(storeCodeSpec);

            if (storeByCode != null)
            {
                _logger.LogDebug("Encontrado por código: {Name}", storeByCode.Name);
                return new List<Warehouse> { storeByCode };
            }

            // 2. Buscar almacenes que contengan el storeId en el nombre
            var storeNameSpec = new WarehouseByNameSpec(storeId.ToString());
            var storesByName = await _unitOfWork.Warehouses.GetListAsync(storeNameSpec);

            if (storesByName.Any())
            {
                _logger.LogDebug("Encontrados {Count} por nombre", storesByName.Count);
                return storesByName;
            }

            // 3. Si no hay específicos, buscar CUALQUIER almacén físico en la misma ciudad
            // (Aquí necesitarías lógica de geolocalización o ciudad)
            var anyPhysicalSpec = new WarehouseByTypeSpec(WarehouseType.Physical);
            var anyPhysical = await _unitOfWork.Warehouses.GetListAsync(anyPhysicalSpec);

            if (anyPhysical.Any())
            {
                _logger.LogDebug("Usando almacén físico genérico");
                return new List<Warehouse> { anyPhysical.First() };
            }

            // 4. Último recurso: cualquier almacén activo
            var anyActiveSpec = new ActiveWarehousesSpec();
            var anyWarehouse = await _unitOfWork.Warehouses.GetBySpecAsync(anyActiveSpec);

            if (anyWarehouse != null)
            {
                _logger.LogWarning("Usando almacén no-físico como fallback: {Name}", anyWarehouse.Name);
                return new List<Warehouse> { anyWarehouse };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo almacenes para tienda {StoreId}", storeId);
        }

        return new List<Warehouse>();
    }

    /// <summary>
    /// Asigna producto desde los almacenes de la tienda
    /// </summary>
    private async Task<AllocationResult> AllocateProduct(
        ProductStockRequest productRequest,
        List<Warehouse> storeWarehouses)
    {
        // Intentar en cada almacén de la tienda
        foreach (var warehouse in storeWarehouses)
        {
            var stockInfo = await GetStockInfo(productRequest.ProductId, warehouse.Id);

            if (stockInfo.HasStock && stockInfo.AvailableQuantity >= productRequest.Quantity)
            {
                return new AllocationResult
                {
                    Success = true,
                    Allocation = new WarehouseAllocation
                    {
                        ProductId = productRequest.ProductId,
                        WarehouseId = warehouse.Id,
                        WarehouseName = warehouse.Name,
                        Quantity = productRequest.Quantity,
                        Location = stockInfo.Location,
                        ShippingCost = 0,
                        EstimatedDays = 0
                    }
                };
            }
        }

        return new AllocationResult
        {
            Success = false,
            ErrorMessage = $"Producto {productRequest.ProductId} sin stock en la tienda"
        };
    }

    /// <summary>
    /// Obtiene información de stock
    /// </summary>
    private async Task<StockInfo> GetStockInfo(Guid productId, Guid warehouseId)
    {
        try
        {
            var spec = new WarehouseProductByWarehouseAndProductSpec(warehouseId, productId);
            var warehouseProduct = await _unitOfWork.WarehouseProducts.GetBySpecAsync(spec);

            return warehouseProduct == null
                ? new StockInfo { HasStock = false }
                : new StockInfo
                {
                    HasStock = true,
                    AvailableQuantity = warehouseProduct.StockQuantity,
                    Location = warehouseProduct.Location
                };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo stock");
            return new StockInfo { HasStock = false };
        }
    }

    private WarehouseSelectionResult CreateErrorResult(string errorMessage)
    {
        return new WarehouseSelectionResult
        {
            Success = false,
            ErrorMessage = errorMessage,
            EstimatedShippingCost = 0,
            EstimatedDeliveryTime = TimeSpan.Zero
        };
    }

    private WarehouseSelectionResult CreateSuccessResult(List<WarehouseAllocation> allocations)
    {
        return new WarehouseSelectionResult
        {
            Success = true,
            Allocations = allocations,
            EstimatedShippingCost = 0,
            EstimatedDeliveryTime = TimeSpan.Zero
        };
    }

    #region Clases Internas
    private class StockInfo
    {
        public bool HasStock { get; set; }
        public int AvailableQuantity { get; set; }
        public string? Location { get; set; }
    }

    private class AllocationResult
    {
        public bool Success { get; set; }
        public WarehouseAllocation? Allocation { get; set; }
        public string? ErrorMessage { get; set; }
    }
    #endregion
}