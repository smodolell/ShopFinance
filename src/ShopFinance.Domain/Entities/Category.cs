namespace ShopFinance.Domain.Entities;

public class Category : BaseEntityAudit<int>
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    public ICollection<Product> Products { get; set; } = new HashSet<Product>();


}
