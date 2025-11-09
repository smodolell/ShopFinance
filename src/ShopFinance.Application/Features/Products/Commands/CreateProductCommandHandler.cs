using ShopFinance.Domain.Entities;

namespace ShopFinance.Application.Features.Products.Commands;

internal class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateProductCommand> _validator;
    private readonly ILogger<CreateProductCommandHandler> _logger;

    public CreateProductCommandHandler(IUnitOfWork unitOfWork, IValidator<CreateProductCommand> validator, ILogger<CreateProductCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }
    public async Task<Result<Guid>> HandleAsync(CreateProductCommand command, CancellationToken cancellationToken = default)
    {
        
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Result.Invalid(validationResult.AsErrors());
        }


        await _unitOfWork.BeginTransactionAsync();

        try
        {


            var product = command.Adapt<Product>();

            await _unitOfWork.Products.AddAsync(product);

            await _unitOfWork.CommitAsync();

            _logger.LogInformation("producto creada exitosamente con ID: {CategoryId}", product.Id);

            return Result.SuccessWithMessage("Producto Creado");
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Error al actualizar la categoría con código: {CategoryCode}", command.Name);
            return Result.Error($"Error al actualizada la categoría: {ex.Message}");

        }
    }
}