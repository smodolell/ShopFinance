using System.ComponentModel;

namespace ShopFinance.Domain.Enums;

public enum ProductState
{
    [Description("Activo")]
    Active,
    [Description("Inactivo")]
    Inactive,
    [Description("Discontinuado")]
    Discontinued
}
