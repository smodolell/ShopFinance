using ShopFinance.Domain.Enums;

internal class ToggleQuotationPlanStatusCommandHandler : ICommandHandler<ToggleQuotationPlanStatusCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ToggleQuotationPlanStatusCommandHandler> _logger;

    public ToggleQuotationPlanStatusCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<ToggleQuotationPlanStatusCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> HandleAsync(ToggleQuotationPlanStatusCommand command, CancellationToken cancellationToken = default)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var quotationPlan = await _unitOfWork.QuotationPlans
                .GetByIdAsync(command.QuotationPlanId, cancellationToken);

            if (quotationPlan == null)
            {
                return Result.Error($"No se encontró el plan de cotización con ID: {command.QuotationPlanId}");
            }

            if (command.IsActive)
            {
                // Activar el plan - extender la fecha de finalización
                quotationPlan.FinalEffectiveDate = DateTime.UtcNow.AddYears(1);
                _logger.LogInformation("Plan de cotización activado: {QuotationPlanId}", quotationPlan.Id);
            }
            else
            {
                // Desactivar el plan - establecer fecha de finalización en el pasado
                quotationPlan.FinalEffectiveDate = DateTime.UtcNow.AddDays(-1);

                // Verificar si el plan está siendo usado
                if (await IsQuotationPlanInUse(quotationPlan.Id, cancellationToken))
                {
                    return Result.Error($"No se puede desactivar el plan '{quotationPlan.PlanName}' porque está siendo utilizado en cotizaciones activas.");
                }

                _logger.LogInformation("Plan de cotización desactivado: {QuotationPlanId}", quotationPlan.Id);
            }

            await _unitOfWork.QuotationPlans.UpdateAsync(quotationPlan, cancellationToken);
            await _unitOfWork.CommitAsync();

            return Result.Success();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Error al cambiar estado del plan de cotización. Comando: {@Command}", command);
            return Result.Error($"Error al cambiar el estado del plan de cotización: {ex.Message}");
        }
    }

    private async Task<bool> IsQuotationPlanInUse(int quotationPlanId, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Quotations
            .AnyAsync(q => q.QuotationPlanId == quotationPlanId &&
                          (q.State == QuotationState.Draft || q.State == QuotationState.InCreditRequest),
                    cancellationToken);
    }
}