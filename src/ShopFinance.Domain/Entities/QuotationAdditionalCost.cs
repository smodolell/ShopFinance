using ShopFinance.Domain.Enums;

namespace ShopFinance.Domain.Entities;

public class QuotationAdditionalCost : BaseEntity<Guid>
{
    public Guid QuotationId { get; set; }
    public CostType CostType { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public bool IsPercentage { get; set; } // Si es porcentaje del monto principal
    public Quotation Quotation { get; set; } = null!;
}
