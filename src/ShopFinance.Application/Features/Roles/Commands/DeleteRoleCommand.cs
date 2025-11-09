namespace ShopFinance.Application.Features.Roles.Commands;

public record DeleteRoleCommand : ICommand<Result>
{
    public Guid RoleId { get; set; }
}
