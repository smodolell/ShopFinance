using ShopFinance.Domain.Enums;

namespace ShopFinance.Domain.Entities;

public class Product : BaseEntityAudit<Guid>
{
    public int CategoryId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string CodeSku { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal CostPrice { get; set; }
    public decimal SalePrice { get; set; }
    public int Stock { get; set; }
    public int StockMin { get; set; }
    public ProductState State { get; set; }

    public virtual Category Category { get; set; } = null!;

}
