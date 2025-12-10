namespace ShopFinance.Application.Services.Frequencies.DTOs;


public class FrequencyListItemDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int DaysInterval { get; set; }
    public int PeriodsPerYear { get; set; }
    public bool IsActive { get; set; }
}