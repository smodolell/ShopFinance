
using ShopFinance.WebApp.Services.Dtos;

namespace ShopFinance.WebApp.Services;

public interface ILayoutService
{
    Task<HashSet<AccessPointDto>> GetMenu();
}
