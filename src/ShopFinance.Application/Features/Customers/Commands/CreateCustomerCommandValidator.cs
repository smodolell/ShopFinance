namespace ShopFinance.Application.Features.Customers.Commands;

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(x => x.Identifier)
           .NotEmpty().WithMessage("El identificador es requerido")
           .Length(5, 20).WithMessage("El identificador debe tener entre 5 y 20 caracteres")
           .Matches(@"^[a-zA-Z0-9_-]+$").WithMessage("El identificador solo puede contener letras, números, guiones y guiones bajos");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("El nombre es requerido")
            .Length(2, 50).WithMessage("El nombre debe tener entre 2 y 50 caracteres")
            .Matches(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$").WithMessage("El nombre solo puede contener letras y espacios");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("El apellido es requerido")
            .Length(2, 50).WithMessage("El apellido debe tener entre 2 y 50 caracteres")
            .Matches(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$").WithMessage("El apellido solo puede contener letras y espacios");

        RuleFor(x => x.Birthdate)
            .NotEmpty().WithMessage("La fecha de nacimiento es requerida")
            .LessThan(DateTime.Now.AddYears(-18)).WithMessage("El cliente debe ser mayor de 18 años")
            .GreaterThan(DateTime.Now.AddYears(-100)).WithMessage("La fecha de nacimiento no puede ser mayor a 100 años");

        // Validación personalizada para nombre completo único
        RuleFor(x => x)
            .MustAsync(BeUniqueFullName).WithMessage("Ya existe un cliente con el mismo nombre y apellido");
    }

    private static async Task<bool> BeUniqueFullName(CreateCustomerCommand command, CancellationToken cancellationToken)
    {
        // Esta validación debería implementarse en el handler con una consulta a la base de datos
        // Por ahora retornamos true, pero en producción deberías inyectar un repositorio aquí
        return await Task.FromResult(true);
    }
}
