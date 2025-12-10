using ShopFinance.Domain.Entities;

namespace ShopFinance.Domain.Services.Dtos;

public class ProductStockRequest
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public Product? Product { get; set; } // Optional, for additional info
}
