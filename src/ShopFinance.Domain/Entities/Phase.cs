using Microsoft.AspNetCore.Identity;

namespace ShopFinance.Domain.Entities;


public class Phase : BaseEntity<int>
{
    public string Code { get; set; } = string.Empty;
    public string PhaseName { get; set; } = string.Empty;
    public string Route { get; set; } = string.Empty;
    public bool IsInitial { get; set; }
    public bool IsFinal { get; set; }
    public bool Required { get; set; }
    public int Order { get; set; }

    public virtual ICollection<PhaseState> States { get; set; } = new HashSet<PhaseState>();

}
