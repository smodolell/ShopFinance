using LiteBus.Commands;
using LiteBus.Extensions.Microsoft.DependencyInjection;
using LiteBus.Queries;
using Microsoft.Extensions.DependencyInjection;
using ShopFinance.Application.Common.Interfaces;
using ShopFinance.Application.Common.Services;
namespace ShopFinance.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<Dummy>();

        services.AddLiteBus(configuration =>
        {
            configuration.AddCommandModule(m => m.RegisterFromAssembly(typeof(DependencyInjection).Assembly)); 
            configuration.AddQueryModule(m => m.RegisterFromAssembly(typeof(DependencyInjection).Assembly));
        });

        services.AddScoped<ISelectListService, SelectListService>();
        services.AddScoped<ILocalizerService, LocalizerService>();

        return services;
    }


}
class Dummy { }
