namespace ShopFinance.Application.Common.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class PhaseAttribute : Attribute
{
    public string Code { get; }
    public string Name { get; }
    public int Order { get; }
    public bool IsInitial { get; set; }
    public bool IsFinal { get; set; }
    public bool IsRequired { get; set; }
    public Type? StatesProviderType { get; set; }

    public PhaseAttribute(string code, string name, int order)
    {
        Code = code;
        Name = name;
        Order = order;
    }
}