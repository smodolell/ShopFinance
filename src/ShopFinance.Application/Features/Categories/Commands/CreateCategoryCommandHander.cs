using ShopFinance.Domain.Entities;

namespace ShopFinance.Application.Features.Categories.Commands;

internal class CreateCategoryCommandHander : ICommandHandler<CreateCategoryCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateCategoryCommand> _validator;
    private readonly ILogger<CreateCategoryCommandHander> _logger;

    public CreateCategoryCommandHander(IUnitOfWork unitOfWork, IValidator<CreateCategoryCommand> validator, ILogger<CreateCategoryCommandHander> logger)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }
    public async Task<Result<int>> HandleAsync(CreateCategoryCommand command, CancellationToken cancellationToken = default)
    {

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Result.Invalid(validationResult.AsErrors());
            }

            var repository = _unitOfWork.Categories;

            bool codeExists = await repository.AnyAsync(c => c.Code == command.Code);
            if (codeExists)
            {
                return Result.Error("Ya existe una categoría con el mismo código.");
            }

            var category = command.Adapt<Category>();

            category.CreatedAt = DateTime.UtcNow;

            await repository.AddAsync(category);

            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Categoría creada exitosamente con ID: {CategoryId}", category.Id);

            return Result.Success(category.Id);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Error al crear la categoría con código: {CategoryCode}", command.Code);
            return Result.Error($"Error al crear la categoría: {ex.Message}");
        }
    }
}
