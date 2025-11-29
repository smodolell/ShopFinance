using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Enums;

namespace ShopFinance.Application.Features.Warehouses.Commands;

public class CreateOrUpdateWarehouseCommand : ICommand<Result<Guid>>
{
    public Guid? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public bool IsActive { get; set; } = true;
    public WarehouseType Type { get; set; } = WarehouseType.Physical;
}
public class CreateOrUpdateWarehouseCommandValidator : AbstractValidator<CreateOrUpdateWarehouseCommand>
{
    public CreateOrUpdateWarehouseCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Warehouse name is required.")
            .MaximumLength(100).WithMessage("Warehouse name must not exceed 100 characters.");
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Warehouse code is required.")
            .MaximumLength(50).WithMessage("Warehouse code must not exceed 50 characters.");
        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Invalid email format.")
            .When(x => !string.IsNullOrEmpty(x.Email));
        RuleFor(x => x.Phone)
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone number format.")
            .When(x => !string.IsNullOrEmpty(x.Phone));
    }
}

internal class CreateOrUpdateWarehouseCommandHandler : ICommandHandler<CreateOrUpdateWarehouseCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateOrUpdateWarehouseCommand> _validator;
    private readonly ILogger<CreateOrUpdateWarehouseCommand> _logger;

    public CreateOrUpdateWarehouseCommandHandler(IUnitOfWork unitOfWork, IValidator<CreateOrUpdateWarehouseCommand> validator, ILogger<CreateOrUpdateWarehouseCommand> logger)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid>> HandleAsync(CreateOrUpdateWarehouseCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Result.Invalid(validationResult.AsErrors());
        }

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            Warehouse? warehouse;
            if (command.Id == null)
            {
                warehouse = new Warehouse
                {
                    Id = Guid.NewGuid(),
                    Name = command.Name,
                    Code = command.Code,
                    Description = command.Description,
                    Address = command.Address,
                    Phone = command.Phone,
                    Email = command.Email,
                    IsActive = command.IsActive,
                    Type = command.Type,
                    CreatedAt = DateTime.UtcNow
                };
                await _unitOfWork.Warehouses.AddAsync(warehouse);
            }
            else
            {

                warehouse = await _unitOfWork.Warehouses.GetByIdAsync(command.Id.Value, cancellationToken);
                if (warehouse == null)
                {
                    return Result.NotFound("Warehouse not found.");
                }
                warehouse.Name = command.Name;
                warehouse.Code = command.Code;
                warehouse.Description = command.Description;
                warehouse.Address = command.Address;
                warehouse.Phone = command.Phone;
                warehouse.Email = command.Email;
                warehouse.IsActive = command.IsActive;
                warehouse.Type = command.Type;
                warehouse.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.Warehouses.UpdateAsync(warehouse);
            }


            await _unitOfWork.CommitAsync();

            return Result.Success(warehouse.Id);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Error al procesar orden");
            return Result.Error($"Error al procesar la orden: {ex.Message}");
        }
    }
}