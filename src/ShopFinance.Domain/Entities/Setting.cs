namespace ShopFinance.Domain.Entities;

public class Setting : BaseEntity<Guid>
{
    public string Key { get; private set; } = string.Empty;

    public string Value { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;
    public bool IsEncrypted { get; private set; }

    public Setting()
    {

    }
    private Setting(string key, string value, string description, bool isEncrypted = false)
    {
        Key = key;
        Value = value;
        Description = description;
        IsEncrypted = isEncrypted;
    }

    public static Setting Create(Guid id, string key, string value, string description, bool isEncrypted = false)
    {
        return new Setting(key, value, description, isEncrypted)
        {
            Id = id,
        };
    }

    public void UpdateValue(string newValue)
    {
        if (string.IsNullOrWhiteSpace(newValue))
            throw new ArgumentException("Value cannot be null or empty");

        Value = newValue;
    }


    public void UpdateValue(string newValue,bool isEncrypted)
    {
        if (string.IsNullOrWhiteSpace(newValue))
            throw new ArgumentException("Value cannot be null or empty");

        Value = newValue;
    }

    public void Encrypt()
    {
        if (!IsEncrypted)
        {
            // Lógica de encriptación
            IsEncrypted = true;
        }
    }
}
