using ShopFinance.Domain.Entities;

namespace ShopFinance.Domain.Specifications;

public class StockTransfersTodaySpec : Specification<StockTransfer>
{
    public StockTransfersTodaySpec()
    {
    }

    public StockTransfersTodaySpec(DateTime today)
    {
        Query.Where(o => o.TransferDate >= today);
    }
}
