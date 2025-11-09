using Microsoft.AspNetCore.Identity;

namespace ShopFinance.Domain.Entities;

public class User : IdentityUser<Guid>, IEntity<Guid>
{
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public bool IsActive { get; set; }

    public User()
    {

    }

    public static User Create(Guid id, string userName, string email, string firstName, string lastName)
    {
        return new User
        {
            Id = id,
            UserName = userName,
            Email = email,
            NormalizedEmail = email.ToUpperInvariant(),
            FirstName = firstName,
            LastName = lastName,
            IsActive = true
        };
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
    public void UpdateProfile(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }
}
