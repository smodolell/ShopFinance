
namespace ShopFinance.Application.Services.TaxRates.DTOs;

public class TaxRateEditDtoValidator : AbstractValidator<TaxRateEditDto>
{
    public TaxRateEditDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre del impuesto es requerido")
            .MaximumLength(50).WithMessage("El nombre no puede exceder 50 caracteres")
            .Matches(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$")
            .WithMessage("El nombre solo puede contener letras y espacios");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("El código es requerido")
            .MaximumLength(10).WithMessage("El código no puede exceder 10 caracteres")
            .Matches(@"^[A-Z][A-Z0-9]{2,9}$").WithMessage("El código debe empezar con letra mayúscula y solo contener letras mayúsculas y números");

        RuleFor(x => x.Percentage)
            .GreaterThanOrEqualTo(0).WithMessage("El porcentaje no puede ser negativo")
            .LessThanOrEqualTo(100).WithMessage("El porcentaje no puede exceder el 100%")
            .PrecisionScale(5, 2,true).WithMessage("El porcentaje debe tener máximo 2 decimales y 5 dígitos totales");

        RuleFor(x => x.EffectiveDate)
            .NotEmpty().WithMessage("La fecha de inicio es requerida")
            .LessThanOrEqualTo(DateTime.Today.AddYears(10))
            .WithMessage("La fecha de inicio no puede ser mayor a 10 años en el futuro");

        RuleFor(x => x)
            .Must(x => !x.ExpirationDate.HasValue || x.ExpirationDate > x.EffectiveDate)
            .WithMessage("La fecha de expiración debe ser posterior a la fecha de inicio");

        RuleFor(x => x.ExpirationDate)
            .Must(date => !date.HasValue || date >= DateTime.Today)
            .When(x => x.ExpirationDate.HasValue)
            .WithMessage("La fecha de expiración no puede ser anterior a hoy");

        // Validar que no haya solapamiento con otras tasas activas del mismo código
        RuleFor(x => x)
            .MustAsync(async (dto, cancellation) =>
                !await HasOverlappingActiveTaxRates(dto))
            .WithMessage("Existen tasas activas del mismo impuesto que se solapan con estas fechas")
            .When(x => x.IsActive);
    }

    private static async Task<bool> HasOverlappingActiveTaxRates(TaxRateEditDto dto)
    {
        // Esta validación se hará en el servicio con UnitOfWork
        return false; // Placeholder - la validación real está en el servicio
    }
}