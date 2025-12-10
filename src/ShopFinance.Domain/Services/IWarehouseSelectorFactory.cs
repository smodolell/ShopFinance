using ShopFinance.Domain.Enums;
using ShopFinance.Domain.Services;

public interface IWarehouseSelectorFactory
{
    IWarehouseSelectorService GetSelector(SaleChannel channel, string? marketplaceName = null);
}
