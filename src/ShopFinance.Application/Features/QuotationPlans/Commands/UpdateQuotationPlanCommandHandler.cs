using Ardalis.GuardClauses;
using ShopFinance.Application.Features.QuotationPlans.DTOs;
using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Specifications;

namespace ShopFinance.Application.Features.QuotationPlans.Commands;

internal class UpdateQuotationPlanCommandHandler : ICommandHandler<UpdateQuotationPlanCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<QuotationPlanEditDto> _validator;
    private readonly ILogger<UpdateQuotationPlanCommandHandler> _logger;

    public UpdateQuotationPlanCommandHandler(
        IUnitOfWork unitOfWork,
        IValidator<QuotationPlanEditDto> validator,
        ILogger<UpdateQuotationPlanCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<int>> HandleAsync(UpdateQuotationPlanCommand command, CancellationToken cancellationToken = default)
    {
        await _unitOfWork.BeginTransactionAsync();

        Guard.Against.Null(command.Model, nameof(command.Model));

        var model = command.Model!;
        try
        {
            if (model.PhaseIdInitial.HasValue)
            {
                var phaseInitial = await _unitOfWork.Phases.GetByIdAsync(model.PhaseIdInitial.Value);
                if (phaseInitial == null)
                {
                    return Result.Error($"La fase inicial con Id '{model.PhaseIdInitial}' no existe.");
                }
                model.Phases.Add(new QuotationPlanPhaseDto
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
                return Result.Error($"El Plan '{model.PlanName}' requiere fase inicial.");
            }
            if (model.PhaseIdFinal.HasValue)
            {
                var phaseFinal = await _unitOfWork.Phases.GetByIdAsync(model.PhaseIdFinal.Value);

                if (phaseFinal == null)
                {
                    return Result.Error($"La fase final con Id '{model.PhaseIdFinal}' no existe.");
                }

                model.Phases.Add(new QuotationPlanPhaseDto
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
                return Result.Error($"El Plan '{model.PlanName}' requiere fase Final.");
            }

            // Validación del comando
            var validationResult = await _validator.ValidateAsync(model, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Result.Invalid(validationResult.AsErrors());
            }

            // Verificar que el TaxRate exista
            var taxRate = await _unitOfWork.TaxRates.GetByIdAsync(model.TaxRateId!.Value, cancellationToken);
            if (taxRate == null)
            {
                return Result.Error($"La tasa de impuesto con ID '{model.TaxRateId}' no existe.");
            }

            // Verificar que el nombre del plan sea único usando Specification
            var existingPlanWithSameName = await _unitOfWork.QuotationPlans
                .GetBySpecAsync(new QuotationPlanByNameSpec(model.PlanName), cancellationToken);

            if (existingPlanWithSameName != null &&
                (command.QuotationPlanId == 0 || existingPlanWithSameName.Id != command.QuotationPlanId))
            {
                return Result.Error($"Ya existe un plan de cotización con el nombre '{model.PlanName}'. Por favor, use un nombre único.");
            }

            // Verificar que las fases existan

            var phaseIds = model.Phases
                .Where(r => r.Active)
                .Select(p => p.PhaseId).ToList();


            var existingPhases = await _unitOfWork.Phases
                .GetListAsync(new PhasesByIdsSpec(phaseIds), cancellationToken);

            if (existingPhases.Count != phaseIds.Count)
            {
                var missingPhaseIds = phaseIds.Except(existingPhases.Select(p => p.Id)).ToList();
                return Result.Error($"Las siguientes fases no existen: {string.Join(", ", missingPhaseIds)}");
            }

            var frequencyIds = model.Frequencies
             .Where(f => f.Active)
             .Select(f => f.FrequencyId).ToList();

            var existingFrequencies = await _unitOfWork.Frequencies
               .GetListAsync(new FrequenciesByIdsSpec(frequencyIds), cancellationToken);

            if (frequencyIds.Any() && existingFrequencies.Count != frequencyIds.Count)
            {
                var missingFrequencyIds = frequencyIds.Except(existingFrequencies.Select(f => f.Id)).ToList();
                return Result.Error($"Las siguientes frecuencias no existen: {string.Join(", ", missingFrequencyIds)}");
            }

            // Verificar que los plazos de pago existan
            var paymentTermIds = model.PaymentTerms
                .Where(pt => pt.Active)
                .Select(pt => pt.PaymentTermId).ToList();

            var existingPaymentTerms = await _unitOfWork.PaymentTerms
              .GetListAsync(new PaymentTermsByIdsSpec(paymentTermIds), cancellationToken);

            if (paymentTermIds.Any() && existingPaymentTerms.Count != paymentTermIds.Count)
            {
                var missingPaymentTermIds = paymentTermIds.Except(existingPaymentTerms.Select(pt => pt.Id)).ToList();
                return Result.Error($"Los siguientes plazos de pago no existen: {string.Join(", ", missingPaymentTermIds)}");
            }

            // Verificar que las tasas de interés existan
            var interestRateIds = model.PaymentTerms
                .Where(pt => pt.Active)
                .Select(pt => pt.InterestRateId!.Value).Distinct().ToList();

            var existingInterestRates = await _unitOfWork.InterestRates
                .GetListAsync(new InterestRatesByIdsSpec(interestRateIds), cancellationToken);

            if (interestRateIds.Any() && existingInterestRates.Count != interestRateIds.Count)
            {
                var missingInterestRateIds = interestRateIds.Except(existingInterestRates.Select(ir => ir.Id)).ToList();
                return Result.Error($"Las siguientes tasas de interés no existen: {string.Join(", ", missingInterestRateIds)}");
            }



            // Actualización de plan existente
            var quotationPlan = await _unitOfWork.QuotationPlans
                       .GetBySpecAsync(
                           new QuotationPlanByIdSpec(command.QuotationPlanId,
                           includePhases: true,
                           includeFrequencies: true,
                           includePaymentTerms: true),
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
            quotationPlan.PlanName = model.PlanName;
            quotationPlan.Code = model.Code;
            quotationPlan.TaxRateId = model.TaxRateId!.Value;
            quotationPlan.IsActive = model.Active;

            if (model.InitialEffectiveDate.HasValue)
            {
                quotationPlan.InitialEffectiveDate = model.InitialEffectiveDate.Value;
            }
            if (model.FinalEffectiveDate.HasValue)
            {
                quotationPlan.FinalEffectiveDate = model.FinalEffectiveDate.Value;
            }


            // Actualizar fases
            UpdateQuotationPlanPhases(quotationPlan, model.Phases, existingPhases);

            // Actualizar frecuencias
            UpdateQuotationPlanFrequencies(quotationPlan, model.Frequencies, existingFrequencies);

            // Actualizar plazos de pago
            UpdateQuotationPlanPaymentTerms(quotationPlan, model.PaymentTerms, existingPaymentTerms, existingInterestRates);

            await _unitOfWork.QuotationPlans.UpdateAsync(quotationPlan, cancellationToken);
            _logger.LogInformation("Plan de cotización actualizado exitosamente con ID: {QuotationPlanId}, Nombre: {PlanName}",
                quotationPlan.Id, quotationPlan.PlanName);

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
        List<QuotationPlanPhaseDto> phaseCommands,
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


    private void UpdateQuotationPlanFrequencies(
        QuotationPlan quotationPlan,
        List<QuotationPlanFrequencyDto> frequencyCommands,
        List<Frequency> existingFrequencies)
    {
        // Eliminar frecuencias que ya no están en el comando
        var frequenciesToRemove = quotationPlan.Frequencies
            .Where(qpf => !frequencyCommands.Any(fc => fc.FrequencyId == qpf.FrequencyId))
            .ToList();

        foreach (var frequencyToRemove in frequenciesToRemove)
        {
            quotationPlan.Frequencies.Remove(frequencyToRemove);
        }

        // Actualizar o agregar frecuencias
        foreach (var frequencyCommand in frequencyCommands.Where(f => f.Active))
        {
            var existingFrequency = quotationPlan.Frequencies
                .FirstOrDefault(qpf => qpf.FrequencyId == frequencyCommand.FrequencyId);

            if (existingFrequency != null)
            {
                // Actualizar frecuencia existente
                existingFrequency.IsDefault = frequencyCommand.IsDefault;
                existingFrequency.Order = frequencyCommand.Order;
            }
            else
            {
                // Agregar nueva frecuencia
                var frequency = existingFrequencies.First(f => f.Id == frequencyCommand.FrequencyId);
                quotationPlan.Frequencies.Add(new QuotationPlanFrequency
                {
                    Id = Guid.NewGuid(),
                    FrequencyId = frequencyCommand.FrequencyId,
                    IsDefault = frequencyCommand.IsDefault,
                    Order = frequencyCommand.Order,
                    QuotationPlan = quotationPlan
                });
            }
        }
    }
    private void UpdateQuotationPlanPaymentTerms(
        QuotationPlan quotationPlan,
        List<QuotationPlanPaymentTermDto> paymentTermCommands,
        List<PaymentTerm> existingPaymentTerms,
        List<InterestRate> existingInterestRates
    )
    {
        // Eliminar plazos que ya no están en el comando
        var paymentTermsToRemove = quotationPlan.PaymentTerms
            .Where(qppt => !paymentTermCommands.Any(ptc => ptc.PaymentTermId == qppt.PaymentTermId))
            .ToList();

        foreach (var paymentTermToRemove in paymentTermsToRemove)
        {
            quotationPlan.PaymentTerms.Remove(paymentTermToRemove);
        }

        // Actualizar o agregar plazos
        foreach (var paymentTermCommand in paymentTermCommands.Where(pt => pt.Active))
        {
            var existingPaymentTerm = quotationPlan.PaymentTerms
                .FirstOrDefault(qppt => qppt.PaymentTermId == paymentTermCommand.PaymentTermId);

            var paymentTerm = existingPaymentTerms.First(pt => pt.Id == paymentTermCommand.PaymentTermId);
            var interestRate = existingInterestRates.First(ir => ir.Id == paymentTermCommand.InterestRateId);

            if (existingPaymentTerm != null)
            {
                // Actualizar plazo existente
                existingPaymentTerm.InterestRateId = interestRate.Id;
                existingPaymentTerm.SpecialRateOverride = paymentTermCommand.SpecialRateOverride;
                existingPaymentTerm.IsPromotional = paymentTermCommand.IsPromotional;
                existingPaymentTerm.PromotionEndDate = paymentTermCommand.PromotionEndDate;
                existingPaymentTerm.Order = paymentTermCommand.Order;
            }
            else
            {
                // Agregar nuevo plazo
                quotationPlan.PaymentTerms.Add(new QuotationPlanPaymentTerm
                {
                    Id = Guid.NewGuid(),
                    PaymentTermId = paymentTermCommand.PaymentTermId,
                    InterestRateId = interestRate.Id,
                    SpecialRateOverride = paymentTermCommand.SpecialRateOverride,
                    IsPromotional = paymentTermCommand.IsPromotional,
                    PromotionEndDate = paymentTermCommand.PromotionEndDate,
                    Order = paymentTermCommand.Order,
                    QuotationPlan = quotationPlan
                });
            }
        }
    }
}