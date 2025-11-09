using ShopFinance.Application.Common.Interfaces;
using ShopFinance.Domain.Entities;

namespace ShopFinance.Application.Features.Users.Commands;

internal class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateUserCommand> _validator;
    private readonly ILocalizerService _localizer;
    private readonly ILogger<CreateUserCommandHandler> _logger;

    public CreateUserCommandHandler(IUnitOfWork unitOfWork,
        IValidator<CreateUserCommand> validator,
        ILocalizerService localizer,
        ILogger<CreateUserCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
        _localizer = localizer;
        _logger = logger;
    }
    public async Task<Result<Guid>> HandleAsync(CreateUserCommand command, CancellationToken cancellationToken = default)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {

            var validationResult = await _validator.ValidateAsync(command);
            if (!validationResult.IsValid)
            {
                return Result.Invalid(validationResult.AsErrors());
            }

            var userExists = await _unitOfWork.Users.AnyAsync(r => r.UserName != null && r.UserName.Equals(command.UserName));
            if (userExists)
            {
                return Result.Conflict(_localizer.GetString("UserAlreadyExists", command.Email));
            }


            var user = User.Create(
                Guid.NewGuid(),
                command.UserName,
                command.Email,
                command.FirstName,
                command.LastName
            );

            await _unitOfWork.Users.AddAsync(user);


            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Usuario creadi exitosamente con ID: {UserId}", user.Id);

            return Result.Success(user.Id);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Error al crear usuario con nombre de usuuario: {UserName}", command.UserName);
            return Result.Error($"Error arear usuario: {ex.Message}");

        }

    }
}
