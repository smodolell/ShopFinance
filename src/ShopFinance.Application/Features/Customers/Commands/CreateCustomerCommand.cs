namespace ShopFinance.Application.Features.Customers.Commands;

public class CreateCustomerCommand : ICommand<Result<Guid>>
{
    public string Identifier { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime? Birthdate { get; set; }
}
