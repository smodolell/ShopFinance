using ShopFinance.Application.Features.QuotationPlans.DTOs;
using ShopFinance.Domain.Specifications;

namespace ShopFinance.Application.Features.QuotationPlans.Queries;

public class GetQuotationEditByIdQuery : IQuery<Result<QuotationPlanEditDto>>
{
    public int QuotationPlanId { get; set; }
}


internal class GetQuotationEditByIdQueryHandler : IQueryHandler<GetQuotationEditByIdQuery, Result<QuotationPlanEditDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetQuotationEditByIdQueryHandler> _logger;

    public GetQuotationEditByIdQueryHandler(IUnitOfWork unitOfWork,ILogger<GetQuotationEditByIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<QuotationPlanEditDto>> HandleAsync(GetQuotationEditByIdQuery message, CancellationToken cancellationToken = default)
    {
        var spec = new QuotationPlanByIdSpec(
            id: message.QuotationPlanId,
            includePhases: true,
            includeFrequencies: true,
            includePaymentTerms: true
        );

        var quotationPlan = await _unitOfWork.QuotationPlans.GetBySpecAsync(spec, cancellationToken);
        if (quotationPlan == null)
        {
            _logger.LogError("El plan de cotización no existe.");
            return Result.NotFound("El plan de cotización no existe.");
        }
        var phasesAll = await _unitOfWork.Phases.GetAllAsync();


        var phases = phasesAll
            .Where(p => !p.IsInitial && !p.IsFinal)
            .OrderBy(p => p.Order).ToList();

        // Include initial and final phases

        var initialPhase = quotationPlan.Phases.FirstOrDefault(p => p.Phase.IsInitial);
        var finalPhase = quotationPlan.Phases.FirstOrDefault(p => p.Phase.IsFinal);

        var dto = new QuotationPlanEditDto
        {
            QuotationPlanId = quotationPlan.Id,
            Code = quotationPlan.Code,
            PlanName = quotationPlan.PlanName,
            TaxRateId = quotationPlan.TaxRateId,
            InitialEffectiveDate = quotationPlan.InitialEffectiveDate,
            FinalEffectiveDate = quotationPlan.FinalEffectiveDate,
            Active = true,
            PhaseIdInitial = initialPhase != null ? initialPhase.Phase.Id : null,
            PhaseIdFinal = finalPhase != null ? finalPhase.Phase.Id : null,
            Phases = phases.Select(p => new QuotationPlanPhaseDto
            {
                PhaseId = p.Id,
                PhaseName = p.PhaseName,
                IsInitial = p.IsInitial,
                IsFinal = p.IsFinal,
                Order = p.Order,
                Required = p.Required,
                Active = quotationPlan.Phases.Any(qp => qp.PhaseId == p.Id),
            }).ToList(),

            Frequencies = quotationPlan.Frequencies.Select(f => new QuotationPlanFrequencyDto
            {
                FrequencyId = f.FrequencyId,
                Active = true,
                IsDefault = f.IsDefault,
                Order = f.Order,

            }).ToList(),
            PaymentTerms = quotationPlan.PaymentTerms.Select(pt => new QuotationPlanPaymentTermDto
            {
                PaymentTermId = pt.PaymentTermId,
                InterestRateId = pt.InterestRateId,
                IsPromotional = pt.IsPromotional,
                Active = true,
                Order = pt.Order,
            }).ToList()
        };

        return Result.Success(dto);
    }
}

