namespace ShopFinance.Application.Features.Users.Commands;

public class UpdateUserCommand : ICommand<Result>
{
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
}


internal class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateUserCommand> _validator;
    private readonly ILogger<UpdateUserCommandHandler> _logger;

    public UpdateUserCommandHandler(IUnitOfWork unitOfWork,IValidator<UpdateUserCommand> validator, ILogger<UpdateUserCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result> HandleAsync(UpdateUserCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Result.Invalid(validationResult.AsErrors());
        }

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            
            var user = await _unitOfWork.Users.GetByIdAsync(command.UserId);
            if (user == null)
            {
                return Result.NotFound("noex");
            }
            command.Adapt(user);

            user.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Categoría actualizada exitosamente con ID: {CategoryId}", user.Id);

            return Result.SuccessWithMessage("Okas");
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Error al actualizar la categoría con código: {CategoryCode}", command.FullName);
            return Result.Error($"Error al actualizada la categoría: {ex.Message}");
        }
   
    }
}

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {

    }
}