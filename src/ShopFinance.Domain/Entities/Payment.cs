namespace ShopFinance.Domain.Entities;
public class Payment : BaseEntity<Guid>
{
    public Guid CreditId { get; set; }
    public DateTime PaymentDate { get; set; }

    // Estos campos representan cuánto del dinero *recibido* se clasificó
    // como Principal, Interest, etc., al momento de la recepción.
    public decimal Principal { get; set; }
    public decimal Interest { get; set; }
    public decimal InterestTax { get; set; }
    public decimal Total { get; set; } // Monto total recibido en este pago

    // Relación de navegación
    public virtual Credit Credit { get; set; } = null!;

    // Referencia a cómo este pago se distribuyó en las cuotas
    public ICollection<PaymentApplication> PaymentApplications { get; set; } = new HashSet<PaymentApplication>();
}
