namespace ShopFinance.Application.Features.Products.Commands;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        // CategoryId - Requerido y válido
        RuleFor(x => x.CategoryId)
            .NotNull().WithMessage("La categoría es requerida")
            .GreaterThan(0).WithMessage("La categoría debe ser válida");

        // Name - Requerido, longitud y formato
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre del producto es requerido")
            .MinimumLength(2).WithMessage("El nombre debe tener al menos 2 caracteres")
            .MaximumLength(200).WithMessage("El nombre no puede exceder 200 caracteres")
            .Matches(@"^[a-zA-Z0-9\sáéíóúÁÉÍÓÚñÑüÜ.,-]+$")
            .WithMessage("El nombre contiene caracteres inválidos");

        // Description - Opcional pero con validaciones si está presente
        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("La descripción no puede exceder 1000 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Description));

        // CostPrice - Mayor que 0 y lógica de negocio
        RuleFor(x => x.CostPrice)
            .GreaterThan(0).WithMessage("El precio de costo debe ser mayor a 0")
            .PrecisionScale(18, 2, false).WithMessage("El precio de costo debe tener máximo 2 decimales")
            .LessThan(1000000).WithMessage("El precio de costo no puede exceder 1,000,000");

        // SalePrice - Mayor que 0 y relación con CostPrice
        RuleFor(x => x.SalePrice)
            .GreaterThan(0).WithMessage("El precio de venta debe ser mayor a 0")
            .PrecisionScale(18, 2, false).WithMessage("El precio de venta debe tener máximo 2 decimales")
            .GreaterThanOrEqualTo(x => x.CostPrice)
            .WithMessage("El precio de venta debe ser mayor o igual al precio de costo")
            .LessThan(1000000).WithMessage("El precio de venta no puede exceder 1,000,000");

        // Stock - No negativo y lógica de negocio
        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0).WithMessage("El stock no puede ser negativo")
            .LessThan(100000).WithMessage("El stock no puede exceder 100,000 unidades");

        // StockMin - No negativo y relación con Stock
        RuleFor(x => x.StockMin)
            .GreaterThanOrEqualTo(0).WithMessage("El stock mínimo no puede ser negativo")
            .LessThan(10000).WithMessage("El stock mínimo no puede exceder 10,000 unidades")
            .LessThanOrEqualTo(x => x.Stock)
            .WithMessage("El stock mínimo no puede ser mayor al stock actual")
            .When(x => x.Stock >= 0);

        // Regla personalizada para márgenes de ganancia
        RuleFor(x => x)
            .Must(HaveReasonableProfitMargin)
            .WithMessage("El margen de ganancia debe ser al menos del 10%")
            .OverridePropertyName("SalePrice");
    }

    private bool HaveReasonableProfitMargin(CreateProductCommand command)
    {
        if (command.CostPrice <= 0 || command.SalePrice <= 0)
            return true; // Ya validado en otras reglas

        var profitMargin = (command.SalePrice - command.CostPrice) / command.CostPrice * 100;
        return profitMargin >= 10; // Mínimo 10% de margen
    }
}
