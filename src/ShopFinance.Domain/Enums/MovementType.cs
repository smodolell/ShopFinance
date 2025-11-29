using System.ComponentModel;

namespace ShopFinance.Domain.Entities;

public enum MovementType
{
    [Description("Entrada de stock")]
    Entry = 0,
    [Description("Salida de stock")]
    Exit = 1,      
    [Description("Ajuste de stock")]
    Adjustment = 2,
    [Description("Transferencia entre almacenes")]
    Transfer = 3   
}
