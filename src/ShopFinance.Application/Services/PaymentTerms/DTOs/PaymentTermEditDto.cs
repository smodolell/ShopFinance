namespace ShopFinance.Application.Services.PaymentTerms.DTOs;

public class PaymentTermEditDto
{
    public int PaymentTermId { get; set; } = 0;
    public string Name { get; set; } = string.Empty; // "3 Meses", "6 Meses", "12 Meses"
    public string Code { get; set; } = string.Empty; // "TERM_3", "TERM_6", "TERM_12"
    public int NumberOfPayments { get; set; } // 3, 6, 12, 24
    public bool IsActive { get; set; } = true;

    // Propiedad calculada
    public int ApproximateDays => NumberOfPayments * 30;
}
