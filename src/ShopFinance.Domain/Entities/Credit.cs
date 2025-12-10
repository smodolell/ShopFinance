using ShopFinance.Domain.Enums;

namespace ShopFinance.Domain.Entities;
public class Credit : BaseEntity<Guid>
{
    public Guid CustomerId { get; set; }
    public int FrequencyId { get; set; }
    public Guid CreditRequestId { get; set; }
    public Guid QuotationId { get; set; }
    public string CreditKey { get; set; } = string.Empty; // Clave de negocio

    /// <summary>
    ///  Tasa de interés
    /// </summary>
    public decimal Rate { get; set; } 

    /// <summary>
    /// Tasa de Impuesto (e.g., IVA Rate)
    /// </summary>
    public decimal TaxRate { get; set; }

    /// <summary>
    /// Plazo (en número de períodos)
    /// </summary>
    public int Term { get; set; } 

    /// <summary>
    /// Monto principal del préstamo (sin intereses/cargos)
    /// </summary>
    public decimal PrincipalAmount { get; set; }

    /// <summary>
    /// Monto total financiado (Principal + cargos financiados)
    /// </summary>
    public decimal FinancedAmount { get; set; }

    public CreditState CreditState { get; set; }
    public virtual Customer Customer { get; set; } = default!;
    public virtual Frequency Frequency { get; set; } = default!;
    public virtual CreditRequest CreditRequest { get; set; } = null!;
    public virtual Quotation Quotation { get; set; } = null!;
    public ICollection<AmortizationScheduleEntry> AmortizationScheduleEntries { get; set; } = new HashSet<AmortizationScheduleEntry>();
    public ICollection<Payment> Payments { get; set; } = new HashSet<Payment>();
}
