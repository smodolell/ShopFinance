
namespace ShopFinance.Domain.Entities;

public class AmortizationScheduleEntry : BaseEntity<Guid>
{
    public Guid CreditId { get; set; }

    /// <summary>
    /// Fecha de Vencimiento
    /// </summary>
    public DateTime DueDate { get; set; } 
    /// <summary>
    /// Capital a amortizar
    /// </summary>
    public decimal Principal { get; set; }
    public decimal Interest { get; set; }
    public decimal InterestTax { get; set; }

    /// <summary>
    /// Cuota programada total (Principal + Interest + InterestTax)
    /// </summary>
    public decimal TotalDue { get; set; }

    /// <summary>
    /// Saldos Pendientes (Balances)
    /// </summary>
    public decimal PrincipalBalance { get; set; }
    public decimal InterestBalance { get; set; }
    public decimal InterestTaxBalance { get; set; }
    /// <summary>
    /// Saldo total pendiente del crédito
    /// </summary>
    public decimal TotalBalance { get; set; } 

    public Credit Credit { get; set; } = null!;

    // Referencia a cómo se aplicaron los pagos a esta cuota
    public ICollection<PaymentApplication> PaymentApplications { get; set; } = new HashSet<PaymentApplication>();
}

