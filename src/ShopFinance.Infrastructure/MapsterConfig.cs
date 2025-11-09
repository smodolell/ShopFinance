using Mapster;
using ShopFinance.Application.Features.Products;

namespace ShopFinance.Infrastructure;

public static class MapsterConfig
{
    public static void Configure()
    {
        Console.WriteLine("=== MAPSTER CONFIG DEBUG ===");
        Console.WriteLine($"Configuraciones ANTES del Scan: {TypeAdapterConfig.GlobalSettings.RuleMap.Count}");

        ConfigureGlobalSettings();


        // Registrar todas las configuraciones de mapeo
        TypeAdapterConfig.GlobalSettings.Scan(
            typeof(ProductsMappingConfig).Assembly,
            typeof(MapsterConfig).Assembly);

        Console.WriteLine($"Configuraciones DESPUÉS del Scan: {TypeAdapterConfig.GlobalSettings.RuleMap.Count}");

        // Listar todas las configuraciones
        foreach (var rule in TypeAdapterConfig.GlobalSettings.RuleMap)
        {
            Console.WriteLine($"Config: {rule.Key.Source.Name} -> {rule.Key.Destination.Name}");
        }

        TypeAdapterConfig.GlobalSettings.Compile();
        Console.WriteLine("=== FIN DEBUG ===");


        // O configurar manualmente
        //TypeAdapterConfig.GlobalSettings.Apply(new RoleMappingConfig());
    }

    private static void ConfigureGlobalSettings()
    {
        // ✅ CONFIGURACIÓN GLOBAL RECOMENDADA
        TypeAdapterConfig.GlobalSettings.Default
            .IgnoreNullValues(true)           // Ignorar propiedades nulas
            .PreserveReference(true)          // Preservar referencias (evita loops)
            .ShallowCopyForSameType(true)     // Copia superficial para mismos tipos
            .MaxDepth(5)                      // Profundidad máxima para evitar loops
            .EnumMappingStrategy(EnumMappingStrategy.ByName); // Mapeo de enums por nombre

        // ✅ Configuración para colecciones
        TypeAdapterConfig.GlobalSettings.Default
            .AddDestinationTransform((string x) => x.Trim()) // Trim automático para strings
            .AddDestinationTransform((decimal x) => Math.Round(x, 2)); // Redondear decimales
    }
}