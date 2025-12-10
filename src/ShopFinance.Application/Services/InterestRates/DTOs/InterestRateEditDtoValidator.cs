namespace ShopFinance.Application.Services.InterestRates.DTOs;

public class InterestRateEditDtoValidator : AbstractValidator<InterestRateEditDto>
{
    public InterestRateEditDtoValidator()
    {
        RuleFor(x => x.RateName)
            .NotEmpty().WithMessage("El nombre de la tasa es requerido")
            .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres");

        RuleFor(x => x.AnnualPercentage)
            .GreaterThanOrEqualTo(0).WithMessage("La tasa no puede ser negativa")
            .LessThanOrEqualTo(100).WithMessage("La tasa no puede exceder el 100%")
            .PrecisionScale(5, 2,true)
            .WithMessage("La tasa debe tener máximo 2 decimales");

        RuleFor(x => x.EffectiveDate)
            .NotEmpty().WithMessage("La fecha de inicio es requerida");

        RuleFor(x => x)
            .Must(x => !x.ExpirationDate.HasValue || x.ExpirationDate > x.EffectiveDate)
            .WithMessage("La fecha de expiración debe ser posterior a la fecha de inicio");

        RuleFor(x => x.ExpirationDate)
            .Must(date => !date.HasValue || date >= DateTime.Today)
            .When(x => x.ExpirationDate.HasValue)
            .WithMessage("La fecha de expiración no puede ser anterior a hoy");
    }
}