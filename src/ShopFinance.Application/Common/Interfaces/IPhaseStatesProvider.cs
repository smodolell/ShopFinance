using ShopFinance.Application.Common.DTOs;

namespace ShopFinance.Application.Common.Interfaces;

public interface IPhaseStatesProvider
{
    IEnumerable<PhaseStateDefinition> GetStates();
}