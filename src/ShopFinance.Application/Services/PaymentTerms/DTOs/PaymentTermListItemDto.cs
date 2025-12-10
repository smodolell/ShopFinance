// PaymentTermEditDto.cs
namespace ShopFinance.Application.Services.PaymentTerms.DTOs;

public class PaymentTermListItemDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public int NumberOfPayments { get; set; }
    public int ApproximateDays => NumberOfPayments * 30;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}