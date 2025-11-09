namespace ShopFinance.Domain.Entities;
public class Frequency : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public virtual ICollection<Credit> Credits { get; set; } = new List<Credit>();
}
