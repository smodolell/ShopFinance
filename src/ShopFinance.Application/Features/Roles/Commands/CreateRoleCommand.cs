namespace ShopFinance.Application.Features.Roles.Commands;

public record CreateRoleCommand : ICommand<Result<Guid>>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
