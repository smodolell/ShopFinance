using ShopFinance.Domain.Entities;

namespace ShopFinance.Application.Features.Roles.Commands;

internal class CreateRoleCommandHandler : ICommandHandler<CreateRoleCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateRoleCommand> _validator;
    private readonly ILogger<CreateRoleCommand> _logger;

    public CreateRoleCommandHandler(IUnitOfWork unitOfWork, IValidator<CreateRoleCommand> validator, ILogger<CreateRoleCommand> logger)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }
    public async Task<Result<Guid>> HandleAsync(CreateRoleCommand command, CancellationToken cancellationToken = default)
    {


        try
        {


            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Result.Invalid(validationResult.AsErrors());
            }


            bool roleExists = await _unitOfWork.Roles.AnyAsync(c => c.Name == command.Name);
            if (roleExists)
            {
                return Result.Error("Ya existe el Rol con el mismo código.");
            }



            var role = Role.Create(
                command.Name,
                command.Description
            );
            if (command.IsActive)
            {
                role.Activate();
            }



            await _unitOfWork.Roles.AddAsync(role);

            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Usuario creado: {RoleName}", command.Name);

            return Result.Success(role.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear el rol con el nombre: {RoleName}", command.Name);
            return Result.Error($"Error al crear el rol: {ex.Message}");

        }

    }
}
