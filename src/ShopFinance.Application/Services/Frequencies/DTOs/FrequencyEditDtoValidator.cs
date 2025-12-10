namespace ShopFinance.Application.Services.Frequencies.DTOs;

public class FrequencyEditDtoValidator : AbstractValidator<FrequencyEditDto>
{
    public FrequencyEditDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre de la frecuencia es requerido")
            .MaximumLength(50).WithMessage("El nombre no puede exceder 50 caracteres");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("El código es requerido")
            .MaximumLength(20).WithMessage("El código no puede exceder 20 caracteres")
            .Matches(@"^[A-Z_]+$").WithMessage("El código solo puede contener letras mayúsculas y guiones bajos");

        RuleFor(x => x.Description)
            .MaximumLength(200).WithMessage("La descripción no puede exceder 200 caracteres");

        RuleFor(x => x.DaysInterval)
            .GreaterThan(0).WithMessage("El intervalo en días debe ser mayor a 0")
            .LessThanOrEqualTo(365).WithMessage("El intervalo en días no puede exceder 365");

        RuleFor(x => x.PeriodsPerYear)
            .GreaterThan(0).WithMessage("Los períodos por año deben ser mayores a 0")
            .LessThanOrEqualTo(365).WithMessage("Los períodos por año no pueden exceder 365");

        // Validación adicional: días * períodos debe ser aproximadamente 365
        RuleFor(x => x)
            .Must(x => Math.Abs(x.DaysInterval * x.PeriodsPerYear - 365) <= 30)
            .WithMessage("Días intervalo × períodos por año debe ser aproximadamente 365 (±30 días)")
            .Unless(x => x.DaysInterval == 0 || x.PeriodsPerYear == 0);
    }
}