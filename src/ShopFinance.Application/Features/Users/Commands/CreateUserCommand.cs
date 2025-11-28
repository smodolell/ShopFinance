using System.ComponentModel;

namespace ShopFinance.Application.Features.Users.Commands;

public class CreateUserCommand : ICommand<Result<Guid>>
{
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string PasswordConfirm { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public string? AvatarUrl { get; set; }
}
