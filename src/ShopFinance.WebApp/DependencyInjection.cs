using BlazorDownloadFile;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.FileProviders;
using MudBlazor;
using MudBlazor.Services;
using ShopFinance.Application.Common.Constants;
using ShopFinance.WebApp.Components;
using ShopFinance.WebApp.Components.Layout.States;
using ShopFinance.WebApp.Middlewares;
using ShopFinance.WebApp.Services;

namespace ShopFinance.WebApp;

public static class DependencyInjection
{

    public static IServiceCollection AddWebApp(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMudServices(config =>
        {
            config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
            config.SnackbarConfiguration.PreventDuplicates = false;
            config.SnackbarConfiguration.NewestOnTop = false;
            config.SnackbarConfiguration.ShowCloseIcon = true;
            config.SnackbarConfiguration.VisibleStateDuration = 4000;
            config.SnackbarConfiguration.HideTransitionDuration = 500;
            config.SnackbarConfiguration.ShowTransitionDuration = 500;
            config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
        });
        services.AddBlazorDownloadFile(ServiceLifetime.Scoped);

        services.AddScoped<ILayoutService, LayoutService>();
        services.AddScoped<ThemeState>();
        services.AddScoped<LayoutState>();

        services.AddScoped<LocalizationCookiesMiddleware>()
       .Configure<RequestLocalizationOptions>(options =>
       {

           options.AddSupportedUICultures(LocalizationConstants.SupportedLanguages.Select(x => x.Code).ToArray());
           options.AddSupportedCultures(LocalizationConstants.SupportedLanguages.Select(x => x.Code).ToArray());
           options.DefaultRequestCulture = new RequestCulture(LocalizationConstants.DefaultLanguageCode);
           options.FallBackToParentUICultures = true;
       })
       .AddLocalization(options => options.ResourcesPath = LocalizationConstants.ResourcesPath);

 


        services.AddHttpClient("ServerAPI", client =>
        {
            client.BaseAddress = new Uri(configuration["App:BaseUrl"] ?? "https://localhost:7124/");
        });
        services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("ServerAPI"));


        services.AddControllers();
        services.AddRazorComponents()
            .AddInteractiveServerComponents();


        return services;
    }

    public static WebApplication UseWebApp(this WebApplication app, IConfiguration config, IWebHostEnvironment webHostEnvironment)
    {

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        var uploadPath = config["FileUpload:UploadPath"] ?? "wwwroot/uploads";
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(
                Path.Combine(webHostEnvironment.ContentRootPath, uploadPath)),
            RequestPath = "/uploads"
        });

        app.UseRouting();

        app.UseAntiforgery();


        app.UseAuthentication();
        app.UseAuthorization();


        var localizationOptions = new RequestLocalizationOptions()
            .SetDefaultCulture(LocalizationConstants.DefaultLanguageCode)
            .AddSupportedCultures(LocalizationConstants.SupportedLanguages.Select(x => x.Code).ToArray())
            .AddSupportedUICultures(LocalizationConstants.SupportedLanguages.Select(x => x.Code).ToArray());

        // Remove AcceptLanguageHeaderRequestCultureProvider to prevent the browser's Accept-Language header from taking effect
        var acceptLanguageProvider = localizationOptions.RequestCultureProviders
            .OfType<AcceptLanguageHeaderRequestCultureProvider>()
            .FirstOrDefault();
        if (acceptLanguageProvider != null)
        {
            localizationOptions.RequestCultureProviders.Remove(acceptLanguageProvider);
        }
        app.UseRequestLocalization(localizationOptions);
        app.UseMiddleware<LocalizationCookiesMiddleware>();

        app.MapControllers();
        app.MapStaticAssets();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        return app;
    }
}
