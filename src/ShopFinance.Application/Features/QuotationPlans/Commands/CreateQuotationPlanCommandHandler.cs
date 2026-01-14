using ShopFinance.Application.Features.QuotationPlans.DTOs;
using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Specifications;

namespace ShopFinance.Application.Features.QuotationPlans.Commands;

internal class CreateQuotationPlanCommandHandler : ICommandHandler<CreateQuotationPlanCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<QuotationPlanEditDto> _validator;
    private readonly ILogger<CreateQuotationPlanCommandHandler> _logger;

    public CreateQuotationPlanCommandHandler(IUnitOfWork unitOfWork,
        IValidator<QuotationPlanEditDto> validator,
        ILogger<CreateQuotationPlanCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<int>> HandleAsync(CreateQuotationPlanCommand command, CancellationToken cancellationToken = default)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var model = command.Model;

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

            if (existingPlanWithSameName != null)
            {
                return Result.Error($"Ya existe un plan de cotización con el nombre '{model.PlanName}'. Por favor, use un nombre único.");
            }

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

            // Creación de nuevo plan
            var quotationPlan = new QuotationPlan
            {
                Id = 0, // Asumir que la base de datos asignará el ID
                Code = model.Code,
                TaxRateId = model.TaxRateId!.Value,
                PlanName = model.PlanName,
                Phases = new HashSet<QuotationPlanPhase>()
            };
            if (model.InitialEffectiveDate.HasValue)
            {
                quotationPlan.InitialEffectiveDate = model.InitialEffectiveDate.Value;
            }
            if (model.FinalEffectiveDate.HasValue)
            {
                quotationPlan.FinalEffectiveDate = model.FinalEffectiveDate.Value;
            }

            // Agregar fases al plan
            foreach (var phaseCommand in model.Phases.Where(r => r.Active))
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

            // Agregar frecuencias al plan
            foreach (var frequencyCommand in model.Frequencies.Where(f => f.Active))
            {
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

            // Agregar plazos de pago al plan
            foreach (var paymentTermCommand in model.PaymentTerms.Where(pt => pt.Active))
            {
                var paymentTerm = existingPaymentTerms.First(pt => pt.Id == paymentTermCommand.PaymentTermId);
                var interestRate = existingInterestRates.First(ir => ir.Id == paymentTermCommand.InterestRateId);

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
            await _unitOfWork.QuotationPlans.AddAsync(quotationPlan, cancellationToken);
            _logger.LogInformation("Plan de cotización creado exitosamente con ID: {QuotationPlanId}, Nombre: {PlanName}",
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
}
