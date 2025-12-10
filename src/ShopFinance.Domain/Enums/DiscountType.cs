using System.ComponentModel;

namespace ShopFinance.Domain.Enums;

public enum DiscountType
{
    [Description("Porcenje")]
    Percentage,
    [Description("Monto Fijo")]
    Fixed
}
