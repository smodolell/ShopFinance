namespace ShopFinance.Application.Features.Users.Commands;

public class CreateUserCommandValdiator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValdiator()
    {
        RuleFor(x => x.UserName).NotEmpty().MinimumLength(3);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
    }
}
