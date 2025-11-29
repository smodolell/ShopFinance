namespace ShopFinance.Domain.Enums;

public enum MovementSource
{
    Purchase = 0,    // Compra de productos
    Sale = 1,        // Venta a cliente
    Manual = 2,      // Ajuste manual
    Production = 3,  // Producción interna
    Damage = 4,      // Daño o pérdida
    Return = 5,      // Devolución de cliente
    Transfer = 6     // Transferencia entre almacenes
}