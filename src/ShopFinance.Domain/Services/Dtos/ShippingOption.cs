namespace ShopFinance.Domain.Services.Dtos;

public class ShippingOption
{
    public string Carrier { get; set; } = string.Empty;
    public string Service { get; set; } = string.Empty;
    public decimal Cost { get; set; }
    public int EstimatedDays { get; set; }
    public DateTime EstimatedDeliveryDate { get; set; }
    public bool Trackable { get; set; }
    public bool Insured { get; set; }
    public decimal InsuranceCost { get; set; }
    public decimal TotalCost => Cost + InsuranceCost;
}
