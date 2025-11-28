using Microsoft.AspNetCore.Identity;

namespace ShopFinance.Domain.Entities;

public class User : IdentityUser<Guid>, IEntity<Guid>
{
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string FullName { get; set; } = string.Empty;

    public string? AvatarUrl { get; set; }
    public bool IsActive { get; set; }

    public User()
    {

    }

    public static User Create(Guid id, string userName, string email, string fullName, string? avatarUrl)
    {
        return new User
        {
            Id = id,
            UserName = userName,
            Email = email,
            NormalizedEmail = email.ToUpperInvariant(),
            FullName = fullName,
            AvatarUrl = avatarUrl,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
    public void UpdateProfile(string fullname, string? avatarUrl)
    {
        FullName = fullname;
        if (!string.IsNullOrEmpty(avatarUrl))
        {
            AvatarUrl = avatarUrl;
        }
    }
}
