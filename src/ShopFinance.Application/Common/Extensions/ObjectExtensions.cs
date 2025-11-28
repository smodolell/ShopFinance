#nullable disable

using System.Text.Json;

namespace ShopFinance.Application.Common.Extensions;



public static class ObjectExtensions
{
    /// <summary>
    /// Clona un objeto usando serialización JSON (copia profunda)
    /// </summary>
    public static T Clone<T>(this T obj)
    {
        if (obj == null) return default(T);

        try
        {
            var json = JsonSerializer.Serialize(obj);
            return JsonSerializer.Deserialize<T>(json);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"No se pudo clonar el objeto de tipo {typeof(T).Name}", ex);
        }
    }

    /// <summary>
    /// Clona un objeto de forma asíncrona (útil para objetos grandes)
    /// </summary>
    public static async Task<T> CloneAsync<T>(this T obj)
    {
        if (obj == null) return default(T);

        return await Task.Run(() => obj.Clone());
    }

    /// <summary>
    /// Clona un objeto con opciones de serialización personalizadas
    /// </summary>
    public static T Clone<T>(this T obj, JsonSerializerOptions options)
    {
        if (obj == null) return default(T);

        var json = JsonSerializer.Serialize(obj, options);
        return JsonSerializer.Deserialize<T>(json, options);
    }
}
