using ShopFinance.Application.Common.DTOs;
using ShopFinance.Application.Common.Interfaces;

namespace ShopFinance.Application.StatesProviders;

public class CreditRequestStatesProvider : IPhaseStatesProvider
{
    public IEnumerable<PhaseStateDefinition> GetStates()
    {
        return new[]
        {
            new PhaseStateDefinition
            {
                Name = "Información General Pendiente",
                IsInitial = true,
                AllowsEdition = true
            },
            new PhaseStateDefinition
            {
                Name = "Información General Completada",
                IsCompleted = true
            }
        };
    }
}
