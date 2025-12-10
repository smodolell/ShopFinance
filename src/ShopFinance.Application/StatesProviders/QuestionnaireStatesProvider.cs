using ShopFinance.Application.Common.DTOs;
using ShopFinance.Application.Common.Interfaces;

namespace ShopFinance.Application.StatesProviders;

public class QuestionnaireStatesProvider : IPhaseStatesProvider
{
    public IEnumerable<PhaseStateDefinition> GetStates()
    {
        return new[]
        {
            new PhaseStateDefinition
            {
                Name = "Cuestionario Pendiente",
                IsInitial = true,
                AllowsEdition = true
            },
            new PhaseStateDefinition
            {
                Name = "Cuestionario Completado",
                IsCompleted = true
            },
            new PhaseStateDefinition
            {
                Name = "Cuestionario Rechazado",
                IsRefused = true
            }
        };
    }
}
