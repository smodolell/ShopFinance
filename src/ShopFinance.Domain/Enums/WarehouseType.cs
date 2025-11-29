using System.ComponentModel;

namespace ShopFinance.Domain.Enums;

public enum WarehouseType
{
    [Description("Almacén físico")]
    Physical = 0,
    [Description("Almacén virtual")]
    Virtual = 1,
    [Description("Almacén en tránsito")]
    Transit = 2
}
