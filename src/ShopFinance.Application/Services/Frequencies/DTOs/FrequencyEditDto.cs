namespace ShopFinance.Application.Services.Frequencies.DTOs;

public class FrequencyEditDto
{
    public int FrequencyId { get; set; } = 0;
    public string Name { get; set; } = string.Empty; // "Mensual", "Quincenal", "Semanal"
    public string Code { get; set; } = string.Empty; // "MONTHLY", "BIWEEKLY", "WEEKLY"
    public string Description { get; set; } = string.Empty;
    public int DaysInterval { get; set; } // 30, 15, 7
    public int PeriodsPerYear { get; set; } // 12, 24, 52
    public bool IsActive { get; set; } = true;
}
