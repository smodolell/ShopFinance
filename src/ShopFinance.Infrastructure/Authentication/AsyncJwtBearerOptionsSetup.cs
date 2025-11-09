//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Options;
//using Microsoft.IdentityModel.Tokens;
//using ShopFinance.Application.Common.Interfaces;

//namespace ShopFinance.Infrastructure.Authentication;

//public class AsyncJwtBearerOptionsSetup : IConfigureNamedOptions<JwtBearerOptions>
//{
//    private readonly IServiceProvider _serviceProvider;
//    private readonly ILogger<AsyncJwtBearerOptionsSetup> _logger;
//    private readonly SemaphoreSlim _configSemaphore = new(1, 1);
//    private bool _isConfigured = false;

//    public AsyncJwtBearerOptionsSetup(
//        IServiceProvider serviceProvider,
//        ILogger<AsyncJwtBearerOptionsSetup> logger)
//    {
//        _serviceProvider = serviceProvider;
//        _logger = logger;
//    }

//    public void Configure(JwtBearerOptions options)
//    {
//        Configure(JwtBearerDefaults.AuthenticationScheme, options);
//    }

//    public void Configure(string? name, JwtBearerOptions options)
//    {
//        if (name != JwtBearerDefaults.AuthenticationScheme)
//            return;

//        // Configuración inicial básica
//        ConfigureBasicOptions(options);

//        // Configurar eventos para carga asíncrona
//        ConfigureBearerEvents(options);

//        _logger.LogInformation("JWT Bearer options initialized with basic configuration");
//    }

//    private void ConfigureBasicOptions(JwtBearerOptions options)
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            // Validación básica inicial
//            ValidateIssuer = false, // Se configurará async
//            ValidateAudience = false, // Se configurará async
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = false, // Se configurará async
//            ClockSkew = TimeSpan.FromMinutes(2) // Tolerancia mayor inicial
//        };

//        options.SaveToken = true;
//        options.RequireHttpsMetadata = false; // Ajustar según entorno
//    }

//    private void ConfigureBearerEvents(JwtBearerOptions options)
//    {
//        options.Events = new JwtBearerEvents
//        {
//            OnMessageReceived = context =>
//            {
//                _logger.LogTrace("JWT message received for path: {Path}", context.Request.Path);
//                return Task.CompletedTask;
//            },

//            OnAuthenticationFailed = async context =>
//            {
//                _logger.LogWarning(context.Exception, "JWT authentication failed");
//                await EnsureFullConfigurationAsync(options);
//            },

//            OnTokenValidated = async context =>
//            {
//                _logger.LogDebug("JWT token validated successfully for user: {User}",
//                    context.Principal?.Identity?.Name);
//                await EnsureFullConfigurationAsync(options);
//            },

//            OnChallenge = async context =>
//            {
//                _logger.LogWarning("JWT authentication challenge for path: {Path}",
//                    context.Request.Path);
//                await EnsureFullConfigurationAsync(options);

//                if (context.AuthenticateFailure != null)
//                {
//                    context.Response.Headers.Append("X-Auth-Failure", "Invalid token");
//                }
//            },

//            OnForbidden = context =>
//            {
//                _logger.LogWarning("Access forbidden for user: {User} to path: {Path}",
//                    context.HttpContext.User?.Identity?.Name, context.Request.Path);
//                return Task.CompletedTask;
//            }
//        };
//    }

//    private async Task EnsureFullConfigurationAsync(JwtBearerOptions options)
//    {
//        if (_isConfigured) return;

//        await _configSemaphore.WaitAsync();
//        try
//        {
//            if (_isConfigured) return;

//            using var scope = _serviceProvider.CreateScope();
//            var configProvider = scope.ServiceProvider.GetRequiredService<IJwtConfigurationProvider>();

//            var validationParameters = await configProvider.GetTokenValidationParametersAsync();

//            // Actualizar solo los parámetros necesarios
//            options.TokenValidationParameters.ValidIssuer = validationParameters.ValidIssuer;
//            options.TokenValidationParameters.ValidateIssuer = true;
//            options.TokenValidationParameters.ValidAudience = validationParameters.ValidAudience;
//            options.TokenValidationParameters.ValidateAudience = true;
//            options.TokenValidationParameters.IssuerSigningKey = validationParameters.IssuerSigningKey;
//            options.TokenValidationParameters.ValidateIssuerSigningKey = true;

//            _isConfigured = true;
//            _logger.LogInformation("JWT Bearer options fully configured with database settings");
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "Failed to fully configure JWT Bearer options from database");
//            // No throw - permitir operación con configuración básica
//        }
//        finally
//        {
//            _configSemaphore.Release();
//        }
//    }
//}