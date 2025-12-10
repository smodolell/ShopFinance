using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShopFinance.Domain.Enums;
using ShopFinance.Domain.Services.Implementations;

namespace ShopFinance.Domain.Services.Dtos;

public class ShippingCalculatorFactory : IShippingCalculatorFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;

    public ShippingCalculatorFactory(
        IServiceProvider serviceProvider,
        IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;
    }

    public IShippingCalculator GetCalculator(string carrier = null, SaleChannel? channel = null)
    {
        //// Si se especifica un carrier, usarlo
        //if (!string.IsNullOrEmpty(carrier))
        //{
        //    return carrier.ToLower() switch
        //    {
        //        "dhl" => _serviceProvider.GetRequiredService<DhlShippingCalculator>(),
        //        "estafeta" => _serviceProvider.GetRequiredService<EstafetaShippingCalculator>(),
        //        "fedex" => _serviceProvider.GetRequiredService<FedexShippingCalculator>(),
        //        "mercadolibre" or "mercadoenvios" =>
        //            _serviceProvider.GetRequiredService<MercadoEnviosShippingCalculator>(),
        //        _ => _serviceProvider.GetRequiredService<BaseShippingCalculator>()
        //    };
        //}

        // Si no, determinar por canal o configuración
        var defaultCarrier = _configuration["Shipping:DefaultCarrier"] ?? "base";

        return GetCalculator(defaultCarrier, channel);
    }
}
