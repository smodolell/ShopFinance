using Microsoft.Extensions.Localization;
using ShopFinance.Application.Common.Interfaces;

namespace ShopFinance.Application.Common.Services;


public class LocalizerService : ILocalizerService
{
    private readonly IStringLocalizerFactory _localizerFactory;

    public LocalizerService(IStringLocalizerFactory localizerFactory)
    {
        _localizerFactory = localizerFactory;
    }

    public string this[string key] => GetString(key);

    public string GetString(string key)
    {
        var localizer = _localizerFactory.Create(typeof(SharedResource));
        return localizer[key];
    }

    public string GetString(string key, params object[] args)
    {
        var localizer = _localizerFactory.Create(typeof(SharedResource));
        var value = localizer[key];
        return string.Format(value, args);
    }

    public string GetString(Type resourceType, string key)
    {
        var localizer = _localizerFactory.Create(resourceType);
        return localizer[key];
    }

    public string GetString(Type resourceType, string key, params object[] args)
    {
        var localizer = _localizerFactory.Create(resourceType);
        var value = localizer[key];
        return string.Format(value, args);
    }
}

// Clase marcadora para recursos compartidos
public class SharedResource { }


