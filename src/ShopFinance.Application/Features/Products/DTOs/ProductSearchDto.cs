namespace ShopFinance.Application.Features.Products.DTOs;

public class ProductSearchDto
{
    public Guid Id { get; set; }
    public Guid WarehouseId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? CodeSku { get; set; }
    public int Stock { get; set; }

}
