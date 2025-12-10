using LiteBus.Commands.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ShopFinance.Application.Features.Phases.Commands;
using System.Reflection;

namespace ShopFinance.Infrastructure.Data.Initializers;


public class SyncPhasesInitializer : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SyncPhasesInitializer> _logger;
    private readonly IConfiguration _configuration;

    public SyncPhasesInitializer(
        IServiceProvider serviceProvider,
        ILogger<SyncPhasesInitializer> logger,
        IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Esperar un tiempo para que la aplicación se inicie completamente
        await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);

        try
        {
            _logger.LogInformation("Iniciando sincronización automática de fases...");

            // Verificar si la sincronización automática está habilitada
            var syncOnStartup = _configuration.GetValue<bool>("Phases:SyncOnStartup", true);

            if (!syncOnStartup)
            {
                _logger.LogInformation("Sincronización automática de fases deshabilitada en configuración.");
                return;
            }

            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<ICommandMediator>();

            // Sincronizar desde el assembly de la API
            var apiAssembly = Assembly.GetExecutingAssembly();
            await SyncPhasesFromAssembly(mediator, apiAssembly, "API", stoppingToken);

            // Intentar sincronizar desde el assembly de Web (Blazor) si existe
            try
            {
                var webAssemblyName = _configuration["Phases:WebAssemblyName"] ?? "ShopFinance.WebApp";
                var webAssembly = Assembly.Load(webAssemblyName);
                await SyncPhasesFromAssembly(mediator, webAssembly, "Web", stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "No se pudo cargar el assembly de Web para sincronización de fases");
            }

            _logger.LogInformation("Sincronización automática de fases completada exitosamente.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error durante la sincronización automática de fases");
        }
    }

    private async Task SyncPhasesFromAssembly(
        ICommandMediator mediator,
        Assembly assembly,
        string sourceName,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Sincronizando fases desde assembly: {AssemblyName} ({Source})",
                assembly.GetName().Name, sourceName);

            var command = new SyncPhasesCommand
            {
                Assembly = assembly
            };

            var result = await mediator.SendAsync(command, cancellationToken);

            if (result.IsSuccess)
            {
                _logger.LogInformation("Sincronización desde {Source} exitosa: {Message}",
                    sourceName, result.SuccessMessage);
            }
            else
            {
                _logger.LogWarning("Sincronización desde {Source} con advertencias: {Message}. Errores: {Errors}",
                    sourceName, result.Errors, string.Join("; ", result.Errors));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sincronizando fases desde {Source}", sourceName);
        }
    }
}
