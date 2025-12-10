using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using ShopFinance.Application.Common.Attributes;
using ShopFinance.Application.Common.Constants;
using ShopFinance.Application.Common.DTOs;
using ShopFinance.Application.Common.Interfaces;
using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Specifications;
using System.Reflection;

namespace ShopFinance.Application.Features.Phases.Commands;

public class SyncPhasesCommand : ICommand<Result>
{
    public Assembly Assembly { get; set; } = null!;
}
public class SyncPhasesCommandHandler : ICommandHandler<SyncPhasesCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<SyncPhasesCommandHandler> _logger;
    private readonly IServiceProvider _serviceProvider;

    public SyncPhasesCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<SyncPhasesCommandHandler> logger,
        IServiceProvider serviceProvider)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public async Task<Result> HandleAsync(SyncPhasesCommand command, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var processedPhases = new List<string>();

            foreach (var component in GetComponentsWithPhaseAttribute(command.Assembly))
            {
                var phaseAttr = GetPhaseAttribute(component);
                var route = GetRoute(component);

                if (phaseAttr == null || string.IsNullOrEmpty(route)) continue;

                await SyncPhaseAsync(phaseAttr, route, cancellationToken);
                processedPhases.Add(phaseAttr.Code);
            }

            // Opcional: Eliminar fases que ya no existen en el código
            await CleanupRemovedPhases(processedPhases, cancellationToken);

            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Sincronizadas {Count} fases: {Phases}",
                processedPhases.Count, string.Join(", ", processedPhases));

            return Result.SuccessWithMessage($"Sincronizadas {processedPhases.Count} fases exitosamente");
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Error al sincronizar fases");
            return Result.Error($"Error al sincronizar fases: {ex.Message}");
        }
    }

    private IEnumerable<Type> GetComponentsWithPhaseAttribute(Assembly assembly)
    {
        return assembly.GetTypes()
            .Where(t => t.IsSubclassOf(typeof(ComponentBase)) &&
                       t.GetCustomAttribute<PhaseAttribute>() != null);
    }

    private async Task SyncPhaseAsync(PhaseAttribute attribute, string route, CancellationToken cancellationToken)
    {
        // Determinar ID (puede ser desde el código o auto-generado)
        var phaseId = PhaseCodes.CodeToId.TryGetValue(attribute.Code, out var id)
            ? id
            : GeneratePhaseId(attribute.Code);

        var spec = new PhaseByCodeSpec(attribute.Code);
        var phase = await _unitOfWork.Phases.GetBySpecAsync(spec, cancellationToken);

        if (phase == null)
        {
            phase = new Phase
            {
                Id = phaseId,
                Code = attribute.Code,
                PhaseName = attribute.Name,
                Route = route,
                Order = attribute.Order,
                IsInitial = attribute.IsInitial,
                IsFinal = attribute.IsFinal,
                Required = attribute.IsRequired,
                States = new HashSet<PhaseState>()
            };
            await _unitOfWork.Phases.AddAsync(phase, cancellationToken);
        }
        else
        {
            // Actualizar propiedades
            phase.PhaseName = attribute.Name;
            phase.Route = route;
            phase.Order = attribute.Order;
            phase.IsInitial = attribute.IsInitial;
            phase.IsFinal = attribute.IsFinal;
            phase.Required = attribute.IsRequired;
            await _unitOfWork.Phases.UpdateAsync(phase, cancellationToken);
        }

        // Sincronizar estados
        await SyncPhaseStatesAsync(phase, attribute, cancellationToken);
    }

    private async Task SyncPhaseStatesAsync(Phase phase, PhaseAttribute attribute, CancellationToken cancellationToken)
    {
        var states = GetPhaseStates(attribute);

        // Eliminar estados que ya no existen
        var statesToRemove = phase.States
            .Where(ps => !states.Any(s => s.Name == ps.PhaseStateName))
            .ToList();

        foreach (var stateToRemove in statesToRemove)
        {
            phase.States.Remove(stateToRemove);
        }

        // Agregar/actualizar estados
        foreach (var stateDef in states)
        {
            var existingState = phase.States.FirstOrDefault(ps => ps.PhaseStateName == stateDef.Name);

            if (existingState == null)
            {
                var phaseState = new PhaseState
                {
                    Id = 0,
                    PhaseStateName = stateDef.Name,
                    Initial = stateDef.IsInitial,
                    Edition = stateDef.AllowsEdition,
                    Completed = stateDef.IsCompleted,
                    Canceled = stateDef.IsCanceled,
                    Refused = stateDef.IsRefused,
                    PreviousPhase = stateDef.ReturnsToPrevious,
                    Phase = phase
                };
                phase.States.Add(phaseState);
            }
            else
            {
                // Actualizar propiedades
                existingState.Initial = stateDef.IsInitial;
                existingState.Edition = stateDef.AllowsEdition;
                existingState.Completed = stateDef.IsCompleted;
                existingState.Canceled = stateDef.IsCanceled;
                existingState.Refused = stateDef.IsRefused;
                existingState.PreviousPhase = stateDef.ReturnsToPrevious;
            }
        }
        await Task.CompletedTask;
    }

    private IEnumerable<PhaseStateDefinition> GetPhaseStates(PhaseAttribute attribute)
    {
        // Si tiene proveedor de estados, usarlo
        if (attribute.StatesProviderType != null)
        {
            var provider = ActivatorUtilities.CreateInstance(_serviceProvider, attribute.StatesProviderType)
                as IPhaseStatesProvider;
            return provider?.GetStates() ?? GetDefaultStates();
        }

        // Estados por defecto basados en el tipo de fase
        return GetDefaultStates();
    }

    private IEnumerable<PhaseStateDefinition> GetDefaultStates()
    {
        return new[]
        {
            new PhaseStateDefinition { Name = "Pendiente", IsInitial = true, AllowsEdition = true },
            new PhaseStateDefinition { Name = "En Proceso", AllowsEdition = true },
            new PhaseStateDefinition { Name = "Completado", IsCompleted = true },
            new PhaseStateDefinition { Name = "Rechazado", IsRefused = true },
            new PhaseStateDefinition { Name = "Cancelado", IsCanceled = true }
        };
    }

    private async Task CleanupRemovedPhases(List<string> currentPhaseCodes, CancellationToken cancellationToken)
    {
        var allPhases = await _unitOfWork.Phases.GetAllAsync();
        var phasesToRemove = allPhases.Where(p => !currentPhaseCodes.Contains(p.Code)).ToList();

        foreach (var phaseToRemove in phasesToRemove)
        {
            // Verificar si la fase está siendo usada
            if (!await IsPhaseInUse(phaseToRemove.Id, cancellationToken))
            {
                await _unitOfWork.Phases.DeleteAsync(phaseToRemove, cancellationToken);
            }
            else
            {
                _logger.LogWarning("No se puede eliminar la fase {PhaseCode} porque está en uso", phaseToRemove.Code);
            }
        }
    }

    private async Task<bool> IsPhaseInUse(int phaseId, CancellationToken cancellationToken)
    {
        // Verificar si hay QuotationPlans usando esta fase
        var plansUsingPhase = await _unitOfWork.QuotationPlans
            .AnyAsync(qp => qp.Phases.Any(qpp => qpp.PhaseId == phaseId), cancellationToken);

        // Verificar si hay CreditRequests en esta fase
        var requestsInPhase = await _unitOfWork.CreditRequests
            .AnyAsync(cr => cr.Phases.Any(crp => crp.PhaseState.PhaseId == phaseId), cancellationToken);

        return plansUsingPhase || requestsInPhase;
    }

    private string GetRoute(Type componentType)
    {
        var routeAttr = componentType.GetCustomAttribute<RouteAttribute>();
        if (routeAttr == null) return string.Empty;

        var route = routeAttr.Template;
        return route.Contains('{')
            ? route.Substring(0, route.IndexOf('{')).TrimEnd('/')
            : route;
    }

    private PhaseAttribute? GetPhaseAttribute(Type componentType)
    {
        return componentType.GetCustomAttribute<PhaseAttribute>();
    }

    private int GeneratePhaseId(string code)
    {
        // Generar un ID único basado en el hash del código
        return Math.Abs(code.GetHashCode());
    }
}
