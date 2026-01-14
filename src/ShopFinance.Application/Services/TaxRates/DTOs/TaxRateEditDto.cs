
namespace ShopFinance.Application.Services.TaxRates.DTOs;

public class TaxRateEditDto
{
    public int TaxRateId { get; set; } = 0;
    public string Name { get; set; } = string.Empty; // "IVA", "IGV", "GST"
    public string Code { get; set; } = string.Empty; // "IVA16", "IGV18", "VAT21"
    public decimal Percentage { get; set; } // 16.0, 18.0, 21.0
    public DateTime? EffectiveDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public bool IsActive { get; set; } = true;
}
