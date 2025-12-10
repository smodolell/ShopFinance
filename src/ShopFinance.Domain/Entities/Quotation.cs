using ShopFinance.Domain.Enums;

namespace ShopFinance.Domain.Entities;

public class Quotation : BaseEntityAudit<Guid>
{
    /// <summary>
    /// Orde de compra asociado a la cotización
    /// </summary>
    public Guid? OrderId { get; set; }

    /// <summary>
    /// Plano de cotización utilizado
    /// </summary>
    public int QuotationPlanId { get; set; }
    /// <summary>
    ///  Frecuencia de pagos
    /// </summary>
    public int FrequencyId { get; set; }

    /// <summary>
    /// Estado de la cotización
    /// </summary>
    public QuotationState State { get; set; }

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
    public decimal TotalAmount { get; set; } // Principal + costos - descuentos
    public decimal MonthlyPayment { get; set; }

    public QuotationPlan QuotationPlan { get; set; } = null!;
    public Order? Order { get; set; }
    public ICollection<QuotationAdditionalCost> AdditionalCosts { get; set; } = new HashSet<QuotationAdditionalCost>();
    public ICollection<QuotationDiscount> Discounts { get; set; } = new HashSet<QuotationDiscount>();
    public ICollection<QuotationAmortizationScheduleEntry> AmortizationScheduleEntries { get; set; } = new HashSet<QuotationAmortizationScheduleEntry>();
}
