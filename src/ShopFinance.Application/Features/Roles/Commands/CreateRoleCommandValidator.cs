namespace ShopFinance.Application.Features.Roles.Commands;

public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
                .WithMessage("El nombre del rol es requerido")
            .MinimumLength(2).WithMessage("El nombre del rol debe tener al menos 2 caracteres")
            .MaximumLength(50).WithMessage("El nombre del rol no puede exceder 50 caracteres")
            .Matches("^[a-zA-Z0-9_]+$").WithMessage("El nombre del rol solo puede contener letras, números y guiones bajos");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("La descripción del rol es requerida")
            .MaximumLength(200).WithMessage("La descripción no puede exceder 200 caracteres");
    }
}