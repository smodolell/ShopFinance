namespace ShopFinance.Domain.Services.Dtos;

public class Address
{
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string Country { get; set; } = "México"; // Default
    public string? Reference { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
}
