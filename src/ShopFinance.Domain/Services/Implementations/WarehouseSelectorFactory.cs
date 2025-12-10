using Microsoft.Extensions.DependencyInjection;
using ShopFinance.Domain.Enums;

namespace ShopFinance.Domain.Services.Implementations;

public class WarehouseSelectorFactory : IWarehouseSelectorFactory
{
    private readonly IServiceProvider _serviceProvider;

    public WarehouseSelectorFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IWarehouseSelectorService GetSelector(SaleChannel channel, string? marketplaceName = null)
    {
        // Para marketplace, podemos tener selectores específicos
        if (!string.IsNullOrEmpty(marketplaceName))
        {
            return marketplaceName.ToLower() switch
            {
                //"mercadolibre" => _serviceProvider.GetRequiredService<MercadoLibreWarehouseSelector>(),
                //"amazon" => _serviceProvider.GetRequiredService<AmazonFBAWarehouseSelector>(),
                //_ => _serviceProvider.GetRequiredService<GenericMarketplaceWarehouseSelector>()
            };
        }

        return channel switch
        {
            SaleChannel.PhysicalPOS => _serviceProvider.GetRequiredService<PhysicalStoreWarehouseSelector>(),
            //SaleChannel.Ecommerce => _serviceProvider.GetRequiredService<EcommerceWarehouseSelector>(),
            //SaleChannel.Marketplace => _serviceProvider.GetRequiredService<GenericMarketplaceWarehouseSelector>(),
            //SaleChannel.Mobile => _serviceProvider.GetRequiredService<MobileAppWarehouseSelector>(),
            _ => throw new NotSupportedException($"Canal {channel} no soportado")
        };
    }
}