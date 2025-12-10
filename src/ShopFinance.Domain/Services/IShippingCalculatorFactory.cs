using ShopFinance.Domain.Enums;

namespace ShopFinance.Domain.Services;

public interface IShippingCalculatorFactory
{
    IShippingCalculator GetCalculator(string carrier = null, SaleChannel? channel = null);
}
