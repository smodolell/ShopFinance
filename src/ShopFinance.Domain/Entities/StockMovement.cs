using ShopFinance.Domain.Enums;

namespace ShopFinance.Domain.Entities;

public class StockMovement : BaseEntityAudit<Guid>
{
    public Guid WarehouseId { get; set; }
    public Guid ProductId { get; set; }
    public Guid? ReferenceId { get; set; } // ID de la entidad que originó el movimiento
    public MovementType MovementType { get; set; }
    public MovementSource Source { get; set; }
    public int Quantity { get; set; }
    public int PreviousStock { get; set; }
    public int NewStock { get; set; }
    public string? Notes { get; set; }
    public DateTime MovementDate { get; set; }

    // Navigation properties
    public Warehouse Warehouse { get; set; } = null!;
    public Product Product { get; set; } = null!;
}
