namespace ShopFinance.Application.Features.QuotationPlans.Commands;

internal class DeleteQuotationPlanCommandHandler : ICommandHandler<DeleteQuotationPlanCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteQuotationPlanCommandHandler> _logger;

    public DeleteQuotationPlanCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<DeleteQuotationPlanCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> HandleAsync(DeleteQuotationPlanCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var quotationPlan = await _unitOfWork.QuotationPlans.GetByIdAsync(command.QuotationPlanId, cancellationToken);
            if (quotationPlan == null)
            {
                return Result.NotFound($"No se encontró el plan de cotización con ID: {command.QuotationPlanId}");
            }

            // Verificar si el plan está siendo usado
            var isInUse = await IsQuotationPlanInUse(command.QuotationPlanId, cancellationToken);
            if (isInUse)
            {
                return Result.Error($"El plan '{quotationPlan.PlanName}' está siendo utilizado y no puede ser eliminado");
            }

            await _unitOfWork.QuotationPlans.DeleteAsync(quotationPlan, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Plan de cotización eliminado: {PlanName}", quotationPlan.PlanName);

            return Result.SuccessWithMessage("Plan de cotización eliminado exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar plan de cotización con ID: {QuotationPlanId}", command.QuotationPlanId);
            return Result.Error($"Error al eliminar el plan de cotización: {ex.Message}");
        }
    }

    private async Task<bool> IsQuotationPlanInUse(int quotationPlanId, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Quotations
            .AnyAsync(q => q.QuotationPlanId == quotationPlanId, cancellationToken);
    }
}