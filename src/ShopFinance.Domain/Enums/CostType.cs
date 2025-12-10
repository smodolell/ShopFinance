using System.ComponentModel;

namespace ShopFinance.Domain.Enums;

public enum CostType
{
    [Description("Comisión")]
    Commission,
    [Description("Administrativo")]
    Administrative,
    [Description("Recargo")]
    Surcharge
}