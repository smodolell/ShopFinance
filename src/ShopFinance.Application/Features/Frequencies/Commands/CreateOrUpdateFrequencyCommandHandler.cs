using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Enums;

namespace ShopFinance.Application.Features.Frequencies.Commands;

internal class CreateOrUpdateFrequencyCommandHandler : ICommandHandler<CreateOrUpdateFrequencyCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateOrUpdateFrequencyCommand> _validator;
    private readonly ILogger<CreateOrUpdateFrequencyCommandHandler> _logger;

    public CreateOrUpdateFrequencyCommandHandler(
        IUnitOfWork unitOfWork,
        IValidator<CreateOrUpdateFrequencyCommand> validator,
        ILogger<CreateOrUpdateFrequencyCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<int>> HandleAsync(CreateOrUpdateFrequencyCommand command, CancellationToken cancellationToken = default)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            // Validación del comando
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Result.Invalid(validationResult.AsErrors());
            }

            // Verificar unicidad del código
            var codeExists = await _unitOfWork.Frequencies
                .AnyAsync(f =>
                    f.Code == command.Code &&
                    (command.FrequencyId == 0 || f.Id != command.FrequencyId),
                    cancellationToken);

            if (codeExists)
            {
                return Result.Error($"Ya existe una frecuencia con el código '{command.Code}'. Por favor, use un código único.");
            }

            // Verificar unicidad del nombre (opcional, según requerimientos)
            var nameExists = await _unitOfWork.Frequencies
                .AnyAsync(f =>
                    f.Name == command.Name &&
                    (command.FrequencyId == 0 || f.Id != command.FrequencyId),
                    cancellationToken);

            if (nameExists)
            {
                return Result.Error($"Ya existe una frecuencia con el nombre '{command.Name}'. Por favor, use un nombre único.");
            }

            Frequency? frequency;

            // Determinar si es creación o actualización
            if (command.FrequencyId == 0)
            {
                // Creación de nueva frecuencia
                frequency = new Frequency
                {
                    Id = 0, // Asignado por la base de datos
                    Name = command.Name,
                    Code = command.Code,
                    Description = command.Description,
                    DaysInterval = command.DaysInterval,
                    PeriodsPerYear = command.PeriodsPerYear
                };

                await _unitOfWork.Frequencies.AddAsync(frequency, cancellationToken);
                _logger.LogInformation("Frecuencia creada exitosamente con ID: {FrequencyId}, Código: {FrequencyCode}",
                    frequency.Id, frequency.Code);
            }
            else
            {
                // Actualización de frecuencia existente
                frequency = await _unitOfWork.Frequencies.GetByIdAsync(command.FrequencyId, cancellationToken);

                if (frequency == null)
                {
                    return Result.Error($"No se encontró la frecuencia con ID: {command.FrequencyId}");
                }

                // Verificar si la frecuencia está siendo usada en cotizaciones o créditos activos
                if (await IsFrequencyInUse(frequency.Id, cancellationToken))
                {
                    return Result.Error($"La frecuencia '{frequency.Name}' está siendo utilizada en cotizaciones o créditos activos y no puede ser modificada.");
                }

                // Actualizar propiedades
                frequency.Name = command.Name;
                frequency.Code = command.Code;
                frequency.Description = command.Description;
                frequency.DaysInterval = command.DaysInterval;
                frequency.PeriodsPerYear = command.PeriodsPerYear;

                await _unitOfWork.Frequencies.UpdateAsync(frequency, cancellationToken);
                _logger.LogInformation("Frecuencia actualizada exitosamente con ID: {FrequencyId}, Código: {FrequencyCode}",
                    frequency.Id, frequency.Code);
            }

            await _unitOfWork.CommitAsync();

            return Result.Success(frequency.Id);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Error al procesar la frecuencia. Comando: {@Command}", command);
            return Result.Error($"Error al procesar la frecuencia: {ex.Message}");
        }
    }

    private async Task<bool> IsFrequencyInUse(int frequencyId, CancellationToken cancellationToken)
    {
        // Verificar si hay cotizaciones usando esta frecuencia
        var hasQuotations = await _unitOfWork.Quotations
            .AnyAsync(q => q.FrequencyId == frequencyId, cancellationToken);

        // Verificar si hay créditos activos usando esta frecuencia
        var hasActiveCredits = await _unitOfWork.Credits
            .AnyAsync(c => c.FrequencyId == frequencyId &&
                          (c.CreditState == CreditState.Activated || c.CreditState == CreditState.Finished),
                    cancellationToken);

        return hasQuotations || hasActiveCredits;
    }
}