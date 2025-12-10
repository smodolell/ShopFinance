namespace ShopFinance.Domain.Entities;
public class Frequency : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int DaysInterval { get; set; } // Ej: 30 para mensual
    public int PeriodsPerYear { get; set; } // Ej: 12 para mensual

    public bool IsActive { get; set; }
    // Relaciones
    public ICollection<Credit> Credits { get; set; } = new HashSet<Credit>();
    public ICollection<Quotation> Quotations { get; set; } = new HashSet<Quotation>();
    public ICollection<QuotationPlanFrequency> QuotationPlanFrequencies { get; set; } = new HashSet<QuotationPlanFrequency>();
}

