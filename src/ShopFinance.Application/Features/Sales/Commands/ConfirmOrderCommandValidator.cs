namespace ShopFinance.Application.Features.Sales.Commands;

public class ConfirmOrderCommandValidator : AbstractValidator<ConfirmOrderCommand>
{
    public ConfirmOrderCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("El ID de la orden es requerido");

        RuleFor(x => x.PaymentMethod)
            .IsInEnum().WithMessage("Método de pago no válido");
    }
}
