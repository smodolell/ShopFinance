using ShopFinance.Domain.Enums;

namespace ShopFinance.Application.Features.StockMovements.DTOs;

public class StockMovementListItemDto
{
    public Guid Id { get; set; }
    public Guid WarehouseId { get; set; }
    public string WarehouseName { get; set; } = string.Empty;
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ProductCode { get; set; } = string.Empty;
    public MovementType MovementType { get; set; }
    public MovementSource Source { get; set; }
    public int Quantity { get; set; }
    public int PreviousStock { get; set; }
    public int NewStock { get; set; }
    public string? Notes { get; set; }
    public DateTime MovementDate { get; set; }
    public DateTime CreatedAt { get; set; }

    // Propiedades calculadas
    public string MovementTypeDisplay => MovementType.ToString();
    public string SourceDisplay => Source.ToString();
    public bool IsPositiveMovement => Quantity > 0;
}

public class StockMovementDetailDto : StockMovementListItemDto
{
    public Guid? ReferenceId { get; set; }
    public string? ReferenceType { get; set; }
    public string? CreatedBy { get; set; }
}