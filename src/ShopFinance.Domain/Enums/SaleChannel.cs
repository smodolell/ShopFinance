using System.ComponentModel;

namespace ShopFinance.Domain.Enums;

public enum SaleChannel
{
    [Description("E-Commerce")]
    Ecommerce = 1,
    [Description("Punto de venta")]
    PhysicalPOS = 2,
    [Description("Marketplace")]
    Marketplace = 3
}