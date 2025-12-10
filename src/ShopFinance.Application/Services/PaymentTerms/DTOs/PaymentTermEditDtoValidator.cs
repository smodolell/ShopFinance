namespace ShopFinance.Application.Services.PaymentTerms.DTOs;

public class PaymentTermEditDtoValidator : AbstractValidator<PaymentTermEditDto>
{
    public PaymentTermEditDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre del plazo es requerido")
            .MaximumLength(50).WithMessage("El nombre no puede exceder 50 caracteres")
            .Matches(@"^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑ\s\-]+$")
            .WithMessage("El nombre contiene caracteres inválidos");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("El código es requerido")
            .MaximumLength(20).WithMessage("El código no puede exceder 20 caracteres")
            .Matches(@"^[A-Z][A-Z0-9_]*$").WithMessage("El código debe empezar con letra mayúscula y solo contener letras mayúsculas, números y guiones bajos");

        RuleFor(x => x.NumberOfPayments)
            .GreaterThan(0).WithMessage("El número de meses debe ser mayor a 0")
            .LessThanOrEqualTo(120).WithMessage("El plazo máximo es de 120 meses (10 años)")
            .Must(months => months % 3 == 0 || months == 1 || months == 2 || months == 6 || months == 12 || months == 18 || months == 24)
            .WithMessage("Los plazos comunes son: 1, 2, 3, 6, 12, 18, 24 meses o múltiplos de 3");

     
    }
}