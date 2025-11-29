namespace ShopFinance.Domain.Services.Dtos;
public record StockCountItem(Guid ProductId, int PhysicalQuantity, string? Notes);
