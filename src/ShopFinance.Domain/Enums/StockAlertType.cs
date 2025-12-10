namespace ShopFinance.Domain.Enums;

public enum StockAlertType
{
    LowStock = 0,     // Stock bajo
    OverStock = 1,    // Stock excedido
    OutOfStock = 2,   // Sin stock
    Expiring = 3      // Producto por expirar
}