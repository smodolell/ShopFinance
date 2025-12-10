namespace ShopFinance.Domain.Entities;

public class QuotationPlanPaymentTerm : BaseEntity<Guid>
{
    public int QuotationPlanId { get; set; }
    public int PaymentTermId { get; set; }
    public int InterestRateId { get; set; }

    // Propiedades específicas de esta combinación plan/plazo
    public decimal? SpecialRateOverride { get; set; } // Si hay tasa especial para este plazo
    public bool IsPromotional { get; set; }
    public DateTime? PromotionEndDate { get; set; }
    public int Order { get; set; } // Para ordenar en UI

    // Navegación
    public QuotationPlan QuotationPlan { get; set; } = null!;
    public PaymentTerm PaymentTerm { get; set; } = null!;
    public InterestRate InterestRate { get; set; } = null!;
}

