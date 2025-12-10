using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Specifications;

namespace ShopFinance.Application.Features.QuotationPlans.Commands;

internal class CreateOrUpdateQuotationPlanCommandHandler : ICommandHandler<CreateOrUpdateQuotationPlanCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateOrUpdateQuotationPlanCommand> _validator;
    private readonly ILogger<CreateOrUpdateQuotationPlanCommandHandler> _logger;

    public CreateOrUpdateQuotationPlanCommandHandler(
        IUnitOfWork unitOfWork,
        IValidator<CreateOrUpdateQuotationPlanCommand> validator,
        ILogger<CreateOrUpdateQuotationPlanCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<int>> HandleAsync(CreateOrUpdateQuotationPlanCommand command, CancellationToken cancellationToken = default)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            if (command.PhaseIdInitial.HasValue)
            {
                var phaseInitial = await _unitOfWork.Phases.GetByIdAsync(command.PhaseIdInitial.Value);
                if (phaseInitial == null)
                {
                    return Result.Error($"La fase inicial con Id '{command.PhaseIdInitial}' no existe.");
                }
                command.Phases.Add(new QuotationPlanPhaseCommand
                {
                    PhaseId = phaseInitial.Id,
                    IsInitial = true,
                    IsFinal = false,
                    Required = true,
                    Order = 1,
                    Active = true
                });
            }
            else
            {
                return Result.Error($"El Plan '{command.PlanName}' requiere fase inicial.");
            }
            if (command.PhaseIdFinal.HasValue)
            {
                var phaseFinal = await _unitOfWork.Phases.GetByIdAsync(command.PhaseIdFinal.Value);

                if (phaseFinal == null)
                {
                    return Result.Error($"La fase final con Id '{command.PhaseIdFinal}' no existe.");
                }

                command.Phases.Add(new QuotationPlanPhaseCommand
                {
                    PhaseId = phaseFinal.Id,
                    IsInitial = false,
                    IsFinal = true,
                    Required = true,
                    Active = true
                });
            }
            else
            {
                return Result.Error($"El Plan '{command.PlanName}' requiere fase Final.");
            }

            // Validación del comando
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Result.Invalid(validationResult.AsErrors());
            }

            // Verificar que el nombre del plan sea único usando Specification
            var existingPlanWithSameName = await _unitOfWork.QuotationPlans
                .GetBySpecAsync(new QuotationPlanByNameSpec(command.PlanName), cancellationToken);

            if (existingPlanWithSameName != null &&
                (command.QuotationPlanId == 0 || existingPlanWithSameName.Id != command.QuotationPlanId))
            {
                return Result.Error($"Ya existe un plan de cotización con el nombre '{command.PlanName}'. Por favor, use un nombre único.");
            }






            // Verificar que las fases existan

            var phaseIds = command.Phases
                .Where(r => r.Active)
                .Select(p => p.PhaseId).ToList();


            var existingPhases = await _unitOfWork.Phases
                .GetListAsync(new PhasesByIdsSpec(phaseIds), cancellationToken);

            if (existingPhases.Count != phaseIds.Count)
            {
                var missingPhaseIds = phaseIds.Except(existingPhases.Select(p => p.Id)).ToList();
                return Result.Error($"Las siguientes fases no existen: {string.Join(", ", missingPhaseIds)}");
            }

            QuotationPlan? quotationPlan;

            // Determinar si es creación o actualización
            if (command.QuotationPlanId == 0)
            {
                // Creación de nuevo plan
                quotationPlan = new QuotationPlan
                {
                    Id = 0, // Asumir que la base de datos asignará el ID
                    PlanName = command.PlanName,
                    Phases = new HashSet<QuotationPlanPhase>()
                };
                if (command.InitialEffectiveDate.HasValue)
                {
                    quotationPlan.InitialEffectiveDate = command.InitialEffectiveDate.Value;
                }
                if (command.FinalEffectiveDate.HasValue)
                {
                    quotationPlan.FinalEffectiveDate = command.FinalEffectiveDate.Value;
                }

                // Agregar fases al plan
                foreach (var phaseCommand in command.Phases.Where(r => r.Active))
                {
                    var phase = existingPhases.First(p => p.Id == phaseCommand.PhaseId);
                    quotationPlan.Phases.Add(new QuotationPlanPhase
                    {
                        Id = Guid.NewGuid(),
                        PhaseId = phaseCommand.PhaseId,
                        Active = phaseCommand.Active,
                        QuotationPlan = quotationPlan
                    });
                }

                await _unitOfWork.QuotationPlans.AddAsync(quotationPlan, cancellationToken);
                _logger.LogInformation("Plan de cotización creado exitosamente con ID: {QuotationPlanId}, Nombre: {PlanName}",
                    quotationPlan.Id, quotationPlan.PlanName);
            }
            else
            {
                // Actualización de plan existente
                quotationPlan = await _unitOfWork.QuotationPlans
                  .GetBySpecAsync(
                      new QuotationPlanByIdSpec(command.QuotationPlanId, includePhases: true),
                      cancellationToken);
                if (quotationPlan == null)
                {
                    return Result.Error($"No se encontró el plan de cotización con ID: {command.QuotationPlanId}");
                }

                // Verificar si el plan está siendo usado en cotizaciones
                if (await IsQuotationPlanInUse(quotationPlan.Id, cancellationToken))
                {
                    return Result.Error($"El plan '{quotationPlan.PlanName}' está siendo utilizado en cotizaciones y no puede ser modificado.");
                }

                // Actualizar propiedades básicas
                quotationPlan.PlanName = command.PlanName;
                if (command.InitialEffectiveDate.HasValue)
                {
                    quotationPlan.InitialEffectiveDate = command.InitialEffectiveDate.Value;
                }
                if (command.FinalEffectiveDate.HasValue)
                {
                    quotationPlan.FinalEffectiveDate = command.FinalEffectiveDate.Value;
                }


                // Actualizar fases
                UpdateQuotationPlanPhases(quotationPlan, command.Phases, existingPhases);

                await _unitOfWork.QuotationPlans.UpdateAsync(quotationPlan, cancellationToken);
                _logger.LogInformation("Plan de cotización actualizado exitosamente con ID: {QuotationPlanId}, Nombre: {PlanName}",
                    quotationPlan.Id, quotationPlan.PlanName);
            }

            await _unitOfWork.CommitAsync();

            return Result.Success(quotationPlan.Id);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Error al procesar el plan de cotización. Comando: {@Command}", command);
            return Result.Error($"Error al procesar el plan de cotización: {ex.Message}");
        }
    }

    private async Task<bool> IsQuotationPlanInUse(int quotationPlanId, CancellationToken cancellationToken)
    {
        // Verificar si hay cotizaciones usando este plan
        return await _unitOfWork.Quotations
            .AnyAsync(q => q.QuotationPlanId == quotationPlanId, cancellationToken);
    }

    private void UpdateQuotationPlanPhases(
        QuotationPlan quotationPlan,
        List<QuotationPlanPhaseCommand> phaseCommands,
        List<Phase> existingPhases)
    {
        // Eliminar fases que ya no están en el comando
        var phasesToRemove = quotationPlan.Phases
            .Where(qpp => !phaseCommands.Any(pc => pc.PhaseId == qpp.PhaseId))
            .ToList();

        foreach (var phaseToRemove in phasesToRemove)
        {
            quotationPlan.Phases.Remove(phaseToRemove);
        }

        // Actualizar o agregar fases
        foreach (var phaseCommand in phaseCommands)
        {
            var existingPhase = quotationPlan.Phases
                .FirstOrDefault(qpp => qpp.PhaseId == phaseCommand.PhaseId);

            if (existingPhase != null)
            {
                // Actualizar fase existente
                existingPhase.Active = phaseCommand.Active;
            }
            else
            {
                // Agregar nueva fase
                var phase = existingPhases.First(p => p.Id == phaseCommand.PhaseId);
                quotationPlan.Phases.Add(new QuotationPlanPhase
                {
                    Id = Guid.NewGuid(),
                    PhaseId = phaseCommand.PhaseId,
                    Active = phaseCommand.Active,
                    QuotationPlan = quotationPlan
                });
            }
        }
    }
}