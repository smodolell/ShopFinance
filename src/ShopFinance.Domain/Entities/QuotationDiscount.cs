using ShopFinance.Domain.Enums;

namespace ShopFinance.Domain.Entities;

public class QuotationDiscount : BaseEntity<Guid>
{
    public Guid QuotationId { get; set; }
    public DiscountType DiscountType { get; set; }
    public string Description { get; set; } = "";
    public decimal Value { get; set; }
    public Quotation Quotation { get; set; } = null!;
}
