using ShopFinance.Application.Features.QuotationPlans.DTOs;
using ShopFinance.Domain.Specifications;

namespace ShopFinance.Application.Features.QuotationPlans.Queries;

internal class GetQuotationPlanByIdQueryHandler : IQueryHandler<GetQuotationPlanByIdQuery, Result<QuotationPlanDetailsDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetQuotationPlanByIdQueryHandler> _logger;

    public GetQuotationPlanByIdQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetQuotationPlanByIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<QuotationPlanDetailsDto>> HandleAsync(
        GetQuotationPlanByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var quotationPlanSpec = new QuotationPlanByIdSpec(query.Id, true);
            var quotationPlan = await _unitOfWork.QuotationPlans.GetBySpecAsync(quotationPlanSpec, cancellationToken);

            if (quotationPlan == null)
            {
                return Result.Error($"No se encontró el plan de cotización con ID: {query.Id}");
            }

            var dto = new QuotationPlanDetailsDto
            {
                Id = quotationPlan.Id,
                PlanName = quotationPlan.PlanName,
                InitialEffectiveDate = quotationPlan.InitialEffectiveDate,
                FinalEffectiveDate = quotationPlan.FinalEffectiveDate,
                Phases = quotationPlan.Phases
                    .Where(qpp => qpp.Active)
                    .Select(qpp => new QuotationPlanPhaseDto
                    {
                        Id = qpp.Id,
                        PhaseId = qpp.PhaseId,
                        PhaseName = qpp.Phase.PhaseName,
                        Active = qpp.Active,
                        Order = qpp.Phase.Order,
                        IsInitial = qpp.Phase.IsInitial,
                        IsFinal = qpp.Phase.IsFinal
                    })
                    .OrderBy(p => p.Order)
                    .ToList()
            };

            _logger.LogInformation("Plan de cotización obtenido exitosamente: {QuotationPlanId}", quotationPlan.Id);

            return Result.Success(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener plan de cotización con ID: {QuotationPlanId}", query.Id);
            return Result.Error($"Error al obtener el plan de cotización: {ex.Message}");
        }
    }
}