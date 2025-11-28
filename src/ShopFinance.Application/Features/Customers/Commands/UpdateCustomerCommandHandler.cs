using ShopFinance.Domain.Specifications;

namespace ShopFinance.Application.Features.Customers.Commands;


internal class UpdateCustomerCommandHandler : ICommandHandler<UpdateCustomerCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateCustomerCommand> _validator;
    private readonly ILogger<UpdateCustomerCommand> _logger;

    public UpdateCustomerCommandHandler(IUnitOfWork unitOfWork, IValidator<UpdateCustomerCommand> validator, ILogger<UpdateCustomerCommand> logger)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result> HandleAsync(UpdateCustomerCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validación fallida al actualizar cliente: {Errors}",
               string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

            return Result.Invalid(validationResult.AsErrors());
        }

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            // Verificar que el cliente existe
            var existingCustomer = await _unitOfWork.Customers
                .GetByIdAsync(command.Id, cancellationToken);

            if (existingCustomer == null)
            {
                _logger.LogWarning("Intento de actualizar cliente no encontrado: {CustomerId}", command.Id);
                return Result.Error($"No se encontró el cliente con ID: {command.Id}");
            }

            // Verificar si el identificador ya existe en otro cliente
            if (existingCustomer.Identifier != command.Identifier)
            {
                var customerWithSameIdentifier = await _unitOfWork.Customers
                    .GetBySpecAsync(new CustomerByIdentifierSpec(command.Identifier), cancellationToken);

                if (customerWithSameIdentifier != null && customerWithSameIdentifier.Id != command.Id)
                {
                    _logger.LogWarning("Intento de actualizar cliente con identificador duplicado: {Identifier}", command.Identifier);
                    return Result.Error($"Ya existe otro cliente con el identificador: {command.Identifier}");
                }
            }

            // Actualizar propiedades
            existingCustomer.Identifier = command.Identifier;
            existingCustomer.FirstName = command.FirstName;
            existingCustomer.LastName = command.LastName;
            existingCustomer.Birthdate = command.Birthdate?? DateTime.Now;

           await  _unitOfWork.Customers.UpdateAsync(existingCustomer);
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Cliente actualizado exitosamente - ID: {CustomerId}, Nombre: {FirstName} {LastName}",
                 existingCustomer.Id, existingCustomer.FirstName, existingCustomer.LastName);

            return Result.SuccessWithMessage("Cliente actualizado exitosamente");
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Error al actualizar el cliente con ID: {CustomerId}", command.Id);
            return Result.Error($"Error al actualizar el cliente: {ex.Message}");
        }
    }
}