namespace ShopFinance.Application.Features.Customers.Commands;

public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("El ID del cliente es requerido");

        RuleFor(x => x.Identifier)
            .NotEmpty().WithMessage("El identificador es requerido")
            .MaximumLength(50).WithMessage("El identificador no puede exceder 50 caracteres");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("El nombre es requerido")
            .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("El apellido es requerido")
            .MaximumLength(100).WithMessage("El apellido no puede exceder 100 caracteres");

        RuleFor(x => x.Birthdate)
            .Must(BeAValidDate).WithMessage("La fecha de nacimiento debe ser válida")
            .When(x => x.Birthdate.HasValue);
    }

    private bool BeAValidDate(DateTime? date)
    {
        return !date.HasValue || (date.Value > DateTime.MinValue && date.Value <= DateTime.Today);
    }
}
