namespace ShopFinance.Domain.Entities;

public class PaymentApplication : BaseEntity<Guid>
{
    public Guid MovementId { get; set; }
    public Guid PaymentId { get; set; }
    public decimal PrincipalPaid { get; set; }
    public decimal InterestPaid { get; set; }
    public decimal InterestTaxPaid { get; set; }
    public decimal TotalTaxPaid { get; set; }
    public Movement Movement { get; set; } = null!;
    public Payment Payment { get; set; } = null!;
}
