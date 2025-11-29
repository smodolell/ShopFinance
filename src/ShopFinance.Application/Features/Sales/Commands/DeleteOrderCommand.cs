namespace ShopFinance.Application.Features.Sales.Commands;

public class DeleteOrderCommand : ICommand<Result>
{
    public Guid OrderId { get; set; }
}
