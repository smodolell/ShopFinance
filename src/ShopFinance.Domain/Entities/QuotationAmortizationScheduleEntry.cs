namespace ShopFinance.Domain.Entities;

public class QuotationAmortizationScheduleEntry : BaseEntity<Guid>
{
    public Guid QuotationId { get; set; }

    /// <summary>
    /// Numero de Período
    /// </summary>
    public int PeriodNumber { get; set; }

    /// <summary>
    /// Fecha de Vencimiento
    /// </summary>
    public DateTime DueDate { get; set; }
    /// <summary>
    /// Capital a amortizar
    /// </summary>
    public decimal Principal { get; set; }
    public decimal Interest { get; set; }
    public decimal InterestTax { get; set; }

    /// <summary>
    /// Cuota programada total (Principal + Interest + InterestTax)
    /// </summary>
    public decimal TotalDue { get; set; }


    // Nuevas colecciones

    public Quotation Quotation { get; set; } = null!;

 
}
