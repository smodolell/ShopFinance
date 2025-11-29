using Ardalis.Result;
using ShopFinance.Application.Features.Sales.DTOs;
using ShopFinance.Domain.Enums;

namespace ShopFinance.Application.Features.Sales.Commands;

public class CreateOrUpdateOrderCommand : ICommand<Result<OrderResultDto>>
{
    public Guid? OrderId { get; set; } // Null para crear, con valor para actualizar
    public Guid? CustomerId { get; set; }
    public string? Notes { get; set; }
    public DateTime? RequiredDate { get; set; }
    public List<OrderItemDto> Items { get; set; } = new();
    public bool ConfirmOrder { get; set; } // Nuevo: true para confirmar, false para guardar borrador
    public string? InvoiceNumber { get; set; }
    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Cash;
}

public class OrderResultDto
{
    public Guid OrderId { get; set; }
    public Guid? SaleId { get; set; } // Solo si se confirmó
    public string OrderNumber { get; set; } = string.Empty;
    public OrderStatus Status { get; set; }
}

public class CreateOrUpdateOrderCommandValidator : AbstractValidator<CreateOrUpdateOrderCommand>
{
    public CreateOrUpdateOrderCommandValidator()
    {
        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("La orden debe tener al menos un item");

        RuleForEach(x => x.Items)
            .ChildRules(item =>
            {
                item.RuleFor(x => x.ProductId)
                    .NotEmpty().WithMessage("El producto es requerido");
                item.RuleFor(x => x.Quantity)
                    .GreaterThan(0).WithMessage("La cantidad debe ser mayor a 0");
                item.RuleFor(x => x.UnitPrice)
                    .GreaterThanOrEqualTo(0).WithMessage("El precio unitario no puede ser negativo");
            });

        // Validaciones específicas para confirmación
        When(x => x.ConfirmOrder, () =>
        {
            RuleFor(x => x.PaymentMethod)
                .IsInEnum().WithMessage("Método de pago no válido");
        });
    }
}
