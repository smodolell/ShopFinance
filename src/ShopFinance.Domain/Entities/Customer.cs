namespace ShopFinance.Domain.Entities;

public class Customer : BaseEntity<Guid>
{
    public string Identifier { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime Birthdate { get; set; }


}
