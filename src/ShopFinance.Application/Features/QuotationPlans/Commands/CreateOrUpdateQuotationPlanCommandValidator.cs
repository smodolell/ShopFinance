using ShopFinance.Domain.Entities;

namespace ShopFinance.Application.Features.QuotationPlans.Commands;

public class CreateOrUpdateQuotationPlanCommandValidator : AbstractValidator<CreateOrUpdateQuotationPlanCommand>
{
    public CreateOrUpdateQuotationPlanCommandValidator()
    {
        // Reglas para PlanName
        RuleFor(x => x.PlanName)
            .NotEmpty().WithMessage("El nombre del plan es requerido")
            .MaximumLength(200).WithMessage("El nombre del plan no puede exceder los 200 caracteres")
            .Matches(@"^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑ\s\-_]+$")
            .WithMessage("El nombre del plan solo puede contener letras, números, espacios, guiones y guiones bajos");

        // Reglas para fechas
        RuleFor(x => x.InitialEffectiveDate)
            .NotEmpty().WithMessage("La fecha de inicio de vigencia es requerida")
            .GreaterThanOrEqualTo(DateTime.Today)
            .WithMessage("La fecha de inicio de vigencia no puede ser anterior a la fecha actual");

        RuleFor(x => x.FinalEffectiveDate)
            .NotEmpty().WithMessage("La fecha de fin de vigencia es requerida")
            .GreaterThan(x => x.InitialEffectiveDate)
            .WithMessage("La fecha de fin de vigencia debe ser posterior a la fecha de inicio");

        // Regla: la vigencia no puede exceder un año (ajustable según negocio)
        //RuleFor(x => x)
        //    .Must(x => (x.FinalEffectiveDate - x.InitialEffectiveDate).Value.TotalDays <= 365)
        //    .WithMessage("El período de vigencia no puede exceder 1 año");

        // Reglas para las fases
        RuleFor(x => x.Phases)
            .NotEmpty().WithMessage("El plan debe contener al menos una fase")
            .Must(phases => phases.Any(p => p.Active))
            .WithMessage("El plan debe tener al menos una fase activa");

        // Validar que no haya fases duplicadas
        RuleFor(x => x.Phases)
            .Must(phases => phases.Select(p => p.PhaseId).Distinct().Count() == phases.Count)
            .WithMessage("No se pueden incluir fases duplicadas en el plan");

        // Validar que haya exactamente una fase inicial y una final (si aplica)
        RuleFor(x => x)
            .MustAsync(async (command, cancellation) =>
            {
                var phaseIds = command.Phases.Select(p => p.PhaseId).ToList();
                var phases = await GetPhasesByIds(phaseIds, cancellation);

                var initialPhases = phases.Where(p => p.IsInitial && command.Phases.Any(cp => cp.PhaseId == p.Id && cp.Active)).Count();
                var finalPhases = phases.Where(p => p.IsFinal && command.Phases.Any(cp => cp.PhaseId == p.Id && cp.Active)).Count();

                // Dependiendo de tus reglas de negocio:
                // return initialPhases == 1 && finalPhases == 1;
                return true; // Ajusta según tus necesidades
            })
            .WithMessage("El plan debe contener exactamente una fase inicial y una fase final");
    }

    private async Task<List<Phase>> GetPhasesByIds(List<int> phaseIds, CancellationToken cancellationToken)
    {
        // Este método debería obtener las fases desde la base de datos
        // Por ahora retornamos una lista vacía - en la implementación real deberías inyectar el repositorio
        return new List<Phase>();
    }
}
