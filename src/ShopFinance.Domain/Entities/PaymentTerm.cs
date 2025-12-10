namespace ShopFinance.Domain.Entities;

public class PaymentTerm : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty; // "3 Meses", "6 Meses", "12 Meses"
    public string Code { get; set; } = string.Empty; // "TERM_3M", "TERM_6M", "TERM_12M"
    public int NumberOfPayments { get; set; } // 3, 6, 12, 24 (número de pagos)
    public bool IsActive { get; set; }
    // Relaciones
    public ICollection<QuotationPlanPaymentTerm> QuotationPlanPaymentTerms { get; set; } = new HashSet<QuotationPlanPaymentTerm>();
}

