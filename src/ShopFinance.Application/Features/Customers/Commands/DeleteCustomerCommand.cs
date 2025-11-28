namespace ShopFinance.Application.Features.Customers.Commands;

public class DeleteCustomerCommand:ICommand<Result>
{
    public Guid CustomerId { get; set; }
}
