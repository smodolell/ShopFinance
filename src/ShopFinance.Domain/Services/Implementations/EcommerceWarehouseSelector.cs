//using Microsoft.Extensions.Logging;
//using ShopFinance.Domain.Entities;
//using ShopFinance.Domain.Enums;
//using ShopFinance.Domain.Services.Dtos;

//namespace ShopFinance.Domain.Services.Implementations;

//public class EcommerceWarehouseSelector : IWarehouseSelectorService
//{
//    private readonly IUnitOfWork _unitOfWork;
//    private readonly IShippingCalculator _shippingCalculator;
//    private readonly ILogger<EcommerceWarehouseSelector> _logger;

//    public EcommerceWarehouseSelector(
//        IUnitOfWork unitOfWork,
//        IShippingCalculator shippingCalculator,
//        ILogger<EcommerceWarehouseSelector> logger)
//    {
//        _unitOfWork = unitOfWork;
//        _shippingCalculator = shippingCalculator;
//        _logger = logger;
//    }

//    public async Task<WarehouseSelectionResult> SelectWarehouseAsync(
//        List<ProductStockRequest> products,
//        SaleContext context)
//    {
//        var result = new WarehouseSelectionResult();

//        if (context.ShippingAddress == null)
//            throw new ArgumentException("ShippingAddress es requerido para e-commerce");

//        // Obtener todos los almacenes disponibles
//        var availableWarehouses = await _unitOfWork.Warehouses
//            .Where(w => w.Type == WarehouseType.Central ||
//                       w.Type == WarehouseType.Regional ||
//                       w.Type == WarehouseType.External)
//            .ToListAsync();

//        // Calcular score para cada almacén
//        var warehouseScores = new List<WarehouseScore>();

//        foreach (var warehouse in availableWarehouses)
//        {
//            var score = await CalculateWarehouseScore(warehouse, products, context);
//            warehouseScores.Add(new WarehouseScore
//            {
//                Warehouse = warehouse,
//                Score = score.TotalScore,
//                CanFulfill = score.CanFulfill,
//                TotalShippingCost = score.ShippingCost,
//                EstimatedDays = score.EstimatedDays
//            });
//        }

//        // Seleccionar el mejor almacén que pueda cumplir
//        var bestWarehouse = warehouseScores
//            .Where(ws => ws.CanFulfill)
//            .OrderByDescending(ws => ws.Score)
//            .FirstOrDefault();

//        if (bestWarehouse == null)
//        {
//            // Si ningún almacén puede cumplir todo, intentar split shipping
//            return await TrySplitShipping(products, context, availableWarehouses);
//        }

//        // Crear asignaciones
//        result.Allocations = products.Select(p => new WarehouseAllocation
//        {
//            ProductId = p.ProductId,
//            WarehouseId = bestWarehouse.Warehouse.Id,
//            WarehouseName = bestWarehouse.Warehouse.Name,
//            Quantity = p.Quantity,
//            ShippingCost = bestWarehouse.TotalShippingCost / products.Count,
//            EstimatedDays = bestWarehouse.EstimatedDays
//        }).ToList();

//        result.Success = true;
//        result.EstimatedShippingCost = bestWarehouse.TotalShippingCost;
//        result.EstimatedDeliveryTime = TimeSpan.FromDays(bestWarehouse.EstimatedDays);

//        return result;
//    }

//    private async Task<WarehouseScoreDetail> CalculateWarehouseScore(
//        Warehouse warehouse,
//        List<ProductStockRequest> products,
//        SaleContext context)
//    {
//        decimal totalScore = 0;
//        bool canFulfill = true;
//        decimal shippingCost = 0;
//        int estimatedDays = 0;

//        // 1. Verificar stock (40% peso)
//        decimal stockScore = 0;
//        foreach (var product in products)
//        {
//            var stock = await _unitOfWork.WarehouseProducts
//                .FirstOrDefaultAsync(wp =>
//                    wp.WarehouseId == warehouse.Id &&
//                    wp.ProductId == product.ProductId);

//            if (stock == null || stock.StockQuantity < product.Quantity)
//            {
//                canFulfill = false;
//                break;
//            }

//            stockScore += (decimal)stock.StockQuantity / (stock.StockMax > 0 ? stock.StockMax : product.Quantity * 10);
//        }

//        if (!canFulfill) return new WarehouseScoreDetail { CanFulfill = false };

//        stockScore = stockScore / products.Count * 100;
//        totalScore += stockScore * 0.4m;

//        // 2. Calcular costo de envío (30% peso)
//        shippingCost = await _shippingCalculator.CalculateCost(
//            warehouse,
//            context.ShippingAddress!,
//            context.OrderTotal);

//        decimal shippingScore = Math.Max(0, 100 - shippingCost * 10); // Invertir costo
//        totalScore += shippingScore * 0.3m;

//        // 3. Calcular tiempo de entrega (20% peso)
//        estimatedDays = await _shippingCalculator.EstimateDeliveryDays(
//            warehouse,
//            context.ShippingAddress!);

//        decimal deliveryScore = Math.Max(0, 100 - estimatedDays * 20);
//        totalScore += deliveryScore * 0.2m;

//        // 4. Prioridad del almacén (10% peso)
//        decimal priorityScore = warehouse.Type switch
//        {
//            WarehouseType.Central => 100,
//            WarehouseType.Regional => 80,
//            WarehouseType.External => 60,
//            _ => 50
//        };
//        totalScore += priorityScore * 0.1m;

//        return new WarehouseScoreDetail
//        {
//            TotalScore = totalScore,
//            CanFulfill = true,
//            ShippingCost = shippingCost,
//            EstimatedDays = estimatedDays
//        };
//    }

//    private async Task<WarehouseSelectionResult> TrySplitShipping(
//        List<ProductStockRequest> products,
//        SaleContext context,
//        List<Warehouse> warehouses)
//    {
//        // Lógica compleja para dividir el envío entre múltiples almacenes
//        // Implementación simplificada:
//        var allocations = new List<WarehouseAllocation>();
//        decimal totalShipping = 0;
//        int maxDays = 0;

//        foreach (var product in products)
//        {
//            var bestWarehouseForProduct = await FindBestWarehouseForProduct(
//                product, warehouses, context);

//            if (bestWarehouseForProduct == null)
//            {
//                return new WarehouseSelectionResult
//                {
//                    Success = false,
//                    ErrorMessage = $"Producto {product.ProductId} no disponible en ningún almacén"
//                };
//            }

//            allocations.Add(bestWarehouseForProduct);
//            totalShipping += bestWarehouseForProduct.ShippingCost;
//            maxDays = Math.Max(maxDays, bestWarehouseForProduct.EstimatedDays);
//        }

//        return new WarehouseSelectionResult
//        {
//            Success = true,
//            Allocations = allocations,
//            EstimatedShippingCost = totalShipping,
//            EstimatedDeliveryTime = TimeSpan.FromDays(maxDays)
//        };
//    }

//    private class WarehouseScore
//    {
//        public Warehouse Warehouse { get; set; } = null!;
//        public decimal Score { get; set; }
//        public bool CanFulfill { get; set; }
//        public decimal TotalShippingCost { get; set; }
//        public int EstimatedDays { get; set; }
//    }

//    private class WarehouseScoreDetail
//    {
//        public decimal TotalScore { get; set; }
//        public bool CanFulfill { get; set; }
//        public decimal ShippingCost { get; set; }
//        public int EstimatedDays { get; set; }
//    }
//}


