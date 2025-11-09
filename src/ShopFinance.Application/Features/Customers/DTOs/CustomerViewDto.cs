namespace ShopFinance.Application.Features.Customers.DTOs;

public class CustomerViewDto
{
    public Guid CustomerId { get; set; }
    public string Identifier { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime Birthdate { get; set; }

}
