namespace ShopFinance.Application.Features.Customers.Commands;

public class UpdateCustomerCommand : ICommand<Result>
{
    public Guid Id { get; set; }
    public string Identifier { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime? Birthdate { get; set; }
}
