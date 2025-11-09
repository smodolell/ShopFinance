namespace ShopFinance.Application.Features.Roles.Commands;

public class UpdateRoleCommand : ICommand<Result>
{
    public Guid RoleId { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}



