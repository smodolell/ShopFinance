namespace ShopFinance.Application.Features.Sales.Commands;


public class CancelSaleCommand : ICommand<Result>
{
    public Guid SaleId { get; set; }
}

