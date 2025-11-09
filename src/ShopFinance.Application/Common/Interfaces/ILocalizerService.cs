namespace ShopFinance.Application.Common.Interfaces;

public interface ILocalizerService
{
    string this[string key] { get; }
    string GetString(string key);
    string GetString(string key, params object[] args);
    string GetString(Type resourceType, string key);
    string GetString(Type resourceType, string key, params object[] args);
}
