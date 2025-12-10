namespace ShopFinance.Domain.Entities;

public class PhaseState : BaseEntity<int>
{
    public int PhaseId { get; set; }
    public string PhaseStateName { get; set; } = string.Empty;
    /// <summary>
    /// Por defecto cuan se entra una fase se toma el estado inicial
    /// </summary>
    public bool Initial { get; set; }
    /// <summary>
    /// Mientras este en este estado se permite la edición
    /// </summary>
    public bool Edition { get; set; }

    /// <summary>
    /// Se completo la fase
    /// </summary>
    public bool Completed { get; set; }

    /// <summary>
    /// Se cancela la fase
    /// </summary>
    public bool Canceled { get; set; }
    /// <summary>
    /// Se rechaza la fase
    /// </summary>
    public bool Refused { get; set; }
    /// <summary>
    /// Vuelve a la fase anterior
    /// </summary>
    public bool PreviousPhase { get; set; }

    public virtual Phase Phase { get; set; } = null!;
}
