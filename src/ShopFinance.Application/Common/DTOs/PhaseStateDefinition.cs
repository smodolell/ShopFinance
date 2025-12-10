namespace ShopFinance.Application.Common.DTOs;

public class PhaseStateDefinition
{
    public string Name { get; set; } = string.Empty;
    public bool IsInitial { get; set; }
    public bool AllowsEdition { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsCanceled { get; set; }
    public bool IsRefused { get; set; }
    public bool ReturnsToPrevious { get; set; }
}

