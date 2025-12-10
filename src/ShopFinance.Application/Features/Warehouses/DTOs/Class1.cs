using ShopFinance.Domain.Enums;

namespace ShopFinance.Application.Features.Warehouses.DTOs;

public class StockTransferDto
{
    public Guid FromWarehouseId { get; set; }
    public Guid ToWarehouseId { get; set; }
    public DateTime TransferDate { get; set; }
    public string? Notes { get; set; }
    public List<StockTransferItemDto> Items { get; set; } = new();
}

public class StockTransferItemDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public string? Notes { get; set; }
}

public class StockCountDto
{
    public Guid WarehouseId { get; set; }
    public DateTime CountDate { get; set; }
    public string? Notes { get; set; }
    public List<StockCountItemDto> Items { get; set; } = new();
}

public class StockCountItemDto
{
    public Guid ProductId { get; set; }
    public int PhysicalQuantity { get; set; }
    public string? Notes { get; set; }
}

public class StockAlertDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ProductCode { get; set; } = string.Empty;
    public Guid WarehouseId { get; set; }
    public string WarehouseName { get; set; } = string.Empty;
    public StockAlertType AlertType { get; set; }
    public int CurrentStock { get; set; }
    public int Threshold { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime AlertDate { get; set; }
}

public class WarehouseSummaryDto
{
    public Guid WarehouseId { get; set; }
    public string WarehouseName { get; set; } = string.Empty;
    public int TotalProducts { get; set; }
    public int LowStockProducts { get; set; }
    public int OutOfStockProducts { get; set; }
    public decimal TotalInventoryValue { get; set; }
    public int TotalMovementsToday { get; set; }
}