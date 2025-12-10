using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ShopFinance.Application.Common.Interfaces;
using ShopFinance.Domain.Common.Interfaces;
using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Repositories;
using ShopFinance.Domain.Services;
using ShopFinance.Domain.Services.Implementations;
using ShopFinance.Infrastructure.Authentication;
using ShopFinance.Infrastructure.Data;
using ShopFinance.Infrastructure.Data.Initializers;
using ShopFinance.Infrastructure.Data.Repositories;
using ShopFinance.Infrastructure.HealthChecks;
using ShopFinance.Infrastructure.Services;

namespace ShopFinance.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // 1. Configuración de Mapster
        MapsterConfig.Configure();
        services.AddMapster();

        // 2. Configuración de Base de Datos
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseSqlite(connectionString);
        }, ServiceLifetime.Scoped);

        services.AddDbContextFactory<ApplicationDbContext>(options =>
        {
            options.UseSqlite(connectionString);
        }, ServiceLifetime.Scoped);

        // 3. Identity Core
        services.AddIdentity<User, Role>(options =>
        {
            // Configuración de Password
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;

            // Opciones adicionales recomendadas
            options.SignIn.RequireConfirmedAccount = false;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
            options.Lockout.MaxFailedAccessAttempts = 5;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        services.AddHttpContextAccessor();
        services.AddScoped<IJwtConfigurationProvider, JwtConfigurationProvider>();
        services.AddScoped<IConfigurationService, ConfigurationService>();
        services.AddScoped<IUploadService, LocalFileUploadService>();


        //Repositorios
        services.AddScoped<IDynamicSorter, DynamicSorter>();
        services.AddScoped<IPaginator, Paginator>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ISettingRepository, SettingRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();


        services.AddScoped<IWarehouseSelectorService, PhysicalStoreWarehouseSelector>();
        services.AddScoped<IStockService, StockService>();

        // 10. Health Checks
        services.AddHealthChecks()
            .AddCheck<JwtConfigurationHealthCheck>("jwt-configuration");

        // 11. Servicios Inicializadores
        services.AddHostedService<UserInitializer>();
        services.AddHostedService<JwtBearerInitializer>();
        services.AddHostedService<SyncPhasesInitializer>();

        return services;
    }
}