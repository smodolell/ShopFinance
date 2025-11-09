namespace ShopFinance.Domain.Entities;

public class Movement : BaseEntity<Guid>
{
    public Guid CreditId { get; set; }

    public decimal Principal { get; set; }
    public decimal Interest { get; set; }
    public decimal InterestTax { get; set; }
    public decimal Total { get; set; }

    public decimal PrincipalBalance { get; set; }
    public decimal InterestBalance { get; set; }
    public decimal InterestTaxBalance { get; set; }
    public decimal TotalBalance { get; set; }

    public virtual Credit Credit { get; set; } = null!;
}