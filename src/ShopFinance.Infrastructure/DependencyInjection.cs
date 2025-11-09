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
            options.Password.RequiredLength = 6;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = false;
            options.User.RequireUniqueEmail = true;

            // Opciones adicionales recomendadas
            options.SignIn.RequireConfirmedAccount = false;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
            options.Lockout.MaxFailedAccessAttempts = 5;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        // 4. Authentication & Authorization
        //services.AddAuthentication(options =>
        //{
        //    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        //})
        //.AddJwtBearer();

        // Configuración asíncrona de JWT
        //services.AddSingleton<IConfigureOptions<JwtBearerOptions>, AsyncJwtBearerOptionsSetup>();
        //services.AddSingleton<IAuthorizationHandler, ApiAuthorizeHandler>();
        //services.AddAuthorizationCore(options =>
        //{
        //    //// Políticas de autorización
        //    //options.AddPolicy("AdminOnly", policy =>
        //    //    policy.RequireRole("Admin"));

        //    //options.AddPolicy("UserOnly", policy =>
        //    //    policy.RequireRole("User", "Admin"));

        //    // Política por defecto
        //    //options.FallbackPolicy = new AuthorizationPolicyBuilder()
        //    //    .RequireAuthenticatedUser()
        //    //    .Build();
        //});
        //services.AddAuthorization(options =>
        //{
        //    options.AddPolicy("ApiAuthorizePolicy", policy =>
        //    {
        //        policy.RequireAuthenticatedUser(); // 要求用户已登录
        //        policy.Requirements.Add(new ApiAuthorizeRequirement());
        //    });
        //});


        // 5. Servicios de Acceso HTTP y Estado
        services.AddHttpContextAccessor();
        //services.AddCascadingAuthenticationState();

        // 6. Servicios de Autenticación JWT
        //services.AddSingleton<IAuthorizationMiddlewareResultHandler, BlazorAuthorizationMiddlewareResultHandler>();
        services.AddScoped<IJwtConfigurationProvider, JwtConfigurationProvider>();
        //services.AddScoped<JwtKeyResolver>();
        //services.AddScoped<IJwtHelper, JwtHelper>();
        //services.AddScoped<ITokenStore, CookieTokenStore>();

        
        

        // 7. Proveedores de Estado de Autenticación
        //services.AddScoped<JwtAuthStateProvider>();
        //services.AddScoped<IAuthStateService>(sp => sp.GetRequiredService<JwtAuthStateProvider>());
        //services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<JwtAuthStateProvider>());

        // 8. Servicios de Aplicación
        //services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IConfigurationService, ConfigurationService>();

        // 9. Repositorios
        services.AddScoped<IDynamicSorter, DynamicSorter>();
        services.AddScoped<IPaginator, Paginator>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ISettingRepository, SettingRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();

        // 10. Health Checks
        services.AddHealthChecks()
            .AddCheck<JwtConfigurationHealthCheck>("jwt-configuration");

        // 11. Servicios Inicializadores
        services.AddHostedService<UserInitializer>();
        services.AddHostedService<JwtBearerInitializer>();

        return services;
    }
}