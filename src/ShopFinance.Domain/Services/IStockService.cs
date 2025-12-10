using Ardalis.Result;
using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Services.Dtos;

namespace ShopFinance.Domain.Services;

public interface IStockService
{
    Task<Result> AdjustStockAsync(Guid warehouseId, Guid productId, int newQuantity, string reason, string? notes = null);
    Task<Result> TransferStockAsync(Guid fromWarehouseId, Guid toWarehouseId, List<ProductQuantity> items, string? notes = null);
    Task<Result> ProcessSaleAsync(Guid warehouseId, List<SaleItem> items);
    //Task<Result> ProcessPurchaseAsync(Guid warehouseId, List<PurchaseItem> items);
    //Task<Result> RegisterStockCountAsync(Guid warehouseId, List<StockCountItem> items, string? notes = null);
    Task<List<StockAlert>> GetStockAlertsAsync(Guid warehouseId);
    Task<WarehouseSummary> GetWarehouseSummaryAsync(Guid warehouseId);
}
