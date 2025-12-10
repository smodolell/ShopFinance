using System.ComponentModel;

namespace ShopFinance.Domain.Enums;

public enum MovementSource
{
    [Description("Compra de productos")]
    Purchase = 0,  
    [Description("Venta a cliente")]
    Sale = 1,     
    [Description("Ajuste manual")]
    Manual = 2,
    [Description("Producción interna")]
    Production = 3, 
    [Description("Daño o pérdida")]
    Damage = 4,     
    [Description("Devolución de cliente")]
    Return = 5,     
    [Description("Transferencia entre almacenes")]
    Transfer = 6    
}