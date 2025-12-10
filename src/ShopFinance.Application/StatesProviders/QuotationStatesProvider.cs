using ShopFinance.Application.Common.DTOs;
using ShopFinance.Application.Common.Interfaces;

namespace ShopFinance.Application.StatesProviders;

public class QuotationStatesProvider : IPhaseStatesProvider
{
    public IEnumerable<PhaseStateDefinition> GetStates()
    {
        return new[]
        {
            new PhaseStateDefinition
            {
                Name = "Cotización Pendiente",
                IsInitial = true,
                AllowsEdition = true
            },
            new PhaseStateDefinition
            {
                Name = "Cotización En Espera",
                ReturnsToPrevious = true
            },
            new PhaseStateDefinition
            {
                Name = "Cotización Completada",
                IsCompleted = true
            },
            new PhaseStateDefinition
            {
                Name = "Cotización Rechazada",
                IsRefused = true
            }
        };
    }
}