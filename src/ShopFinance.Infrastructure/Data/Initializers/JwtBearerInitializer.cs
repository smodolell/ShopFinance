using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ShopFinance.Application.Common.Interfaces;

namespace ShopFinance.Infrastructure.Data.Initializers;
internal class JwtBearerInitializer : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public JwtBearerInitializer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            try
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<JwtBearerInitializer>>();

                // Migraciones de BD
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                await dbContext.Database.MigrateAsync();
                logger.LogInformation("Database migrations applied successfully");

                // Configuraciones por defecto
                var configurationService = scope.ServiceProvider.GetRequiredService<IConfigurationService>();
                await configurationService.InitializeDefaultSettingsAsync();
                logger.LogInformation("Default settings initialized successfully");

                // Verificar JWT
                var jwtConfigProvider = scope.ServiceProvider.GetRequiredService<IJwtConfigurationProvider>();
                var jwtParams = await jwtConfigProvider.GetTokenValidationParametersAsync();
                logger.LogInformation("JWT configuration validated successfully");

            }
            catch (Exception ex)
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<JwtBearerInitializer>>();
                logger.LogError(ex, "An error occurred during application startup initialization");
                throw;
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}