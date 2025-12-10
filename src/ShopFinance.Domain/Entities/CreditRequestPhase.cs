namespace ShopFinance.Domain.Entities;

public class CreditRequestPhase : BaseEntity<Guid>
{
    public Guid CreditRequestId { get; set; }
    public int PhaseStateId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public CreditRequest CreditRequest { get; set; } = null!;
    public PhaseState PhaseState { get; set; } = null!;
}
