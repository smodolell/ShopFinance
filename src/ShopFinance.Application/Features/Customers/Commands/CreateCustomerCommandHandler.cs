using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Specifications;

namespace ShopFinance.Application.Features.Customers.Commands;

internal class CreateCustomerCommandHandler : ICommandHandler<CreateCustomerCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateCustomerCommand> _validator;
    private readonly ILogger<CreateCustomerCommand> _logger;

    public CreateCustomerCommandHandler(IUnitOfWork unitOfWork, IValidator<CreateCustomerCommand> validator, ILogger<CreateCustomerCommand> logger)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }
    public async Task<Result<Guid>> HandleAsync(CreateCustomerCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validación fallida al crear cliente: {Errors}",
               string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

            return Result.Invalid(validationResult.AsErrors());
        }


        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var existingCustomer = await _unitOfWork.Customers
               .GetBySpecAsync(new CustomerByIdentifierSpec(command.Identifier), cancellationToken);

            if (existingCustomer != null)
            {
                _logger.LogWarning("Intento de crear cliente con identificador duplicado: {Identifier}", command.Identifier);
                return Result.Error($"Ya existe un cliente con el identificador: {command.Identifier}");
            }

            var customer = command.Adapt<Customer>();

            await _unitOfWork.Customers.AddAsync(customer);

            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Cliente creado exitosamente - ID: {CustomerId}, Nombre: {FirstName} {LastName}",
                 customer.Id, customer.FirstName, customer.LastName);

            return Result.Success(customer.Id, "Cliente creado exitosamente");
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Error al crear el cliente con identificador: {Identifier}", command.Identifier);
            return Result.Error($"Error al crear el cliente: {ex.Message}");
        }
    }
}