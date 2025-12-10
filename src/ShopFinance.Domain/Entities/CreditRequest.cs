namespace ShopFinance.Domain.Entities;

/// <summary>
/// Solicitud de crédito basada en una cotización aprobada
/// </summary>
public class CreditRequest : BaseEntityAudit<Guid>
{
    public Guid QuotationId { get; set; }
    public Guid? CreditId { get; set; }
    public int PhaseStateId { get; set; }
    public Quotation Quotation { get; set; } = null!;
    /// <summary>
    /// Estado actual de la fase 
    /// </summary>
    public PhaseState PhaseState { get; set; } = null!;
    public Credit? Credit { get; set; } 

    public ICollection<CreditRequestPhase> Phases { get; set; } = new List<CreditRequestPhase>();
}
