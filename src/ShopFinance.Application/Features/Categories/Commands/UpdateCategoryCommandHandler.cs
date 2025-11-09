namespace ShopFinance.Application.Features.Categories.Commands;

internal class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateCategoryCommand> _validator;
    private readonly ILogger<UpdateCategoryCommandHandler> _logger;

    public UpdateCategoryCommandHandler(IUnitOfWork unitOfWork, IValidator<UpdateCategoryCommand> validator, ILogger<UpdateCategoryCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }
    public async Task<Result> HandleAsync(UpdateCategoryCommand command, CancellationToken cancellationToken = default)
    {


        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Result.Invalid(validationResult.AsErrors());
        }

        await _unitOfWork.BeginTransactionAsync();

        try
        {


            var category = await _unitOfWork.Categories.GetByIdAsync(command.CategoryId);
            if (category == null)
            {
                return Result.NotFound($"Categoría con ID {command.CategoryId} no encontrada.");
            }

            bool codeExists = await _unitOfWork.Categories.AnyAsync(c => c.Code == command.Code && c.Id != command.CategoryId);
            if (codeExists)
            {
                return Result.Error("Ya existe otra categoría con el mismo código.");
            }
            command.Adapt(category);
            await _unitOfWork.Categories.UpdateAsync(category);
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Categoría actualizada exitosamente con ID: {CategoryId}", category.Id);

            return Result.Success();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Error al actualizar la categoría con código: {CategoryCode}", command.Code);
            return Result.Error($"Error al actualizada la categoría: {ex.Message}");
        }
    }
}

