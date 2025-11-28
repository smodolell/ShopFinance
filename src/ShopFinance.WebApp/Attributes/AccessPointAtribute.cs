using MudBlazor;
using ShopFinance.WebApp.Constants;

namespace ShopFinance.WebApp.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class AccessPointAttribute : Attribute
{
    public string Menu { get; set; } = "";
    public string ItemMenu { get; set; } = "";

    public string MenuIcon => _menuIcon();

    public AccessPointType AccessPointType { get; set; }

    public bool IsClient { get; set; }

    public AccessPointAttribute(string menu, string itemMenu)
    {
        Menu = menu;
        ItemMenu = itemMenu;
        AccessPointType = AccessPointType.LeftMenu;
        IsClient = false;
    }

    public AccessPointAttribute(string menu, string itemMenu, AccessPointType accessPointType)
    {
        Menu = menu;
        ItemMenu = itemMenu;
        AccessPointType = accessPointType;
        IsClient = false;
    }

    public AccessPointAttribute(string menu, string itemMenu, AccessPointType accessPointType, bool isClient) : this(menu, itemMenu, accessPointType)
    {
        IsClient = isClient;
    }

    private string _menuIcon()
    {
        switch (Menu)
        {
            case AppMenu.MenuConfiguracion:
                return AppMenu.MenuConfiguracionIcon;
            case AppMenu.MenuSystem:
                return AppMenu.MenuSystemIcon;
            case AppMenu.MenuSecurity:
                return AppMenu.MenuSecurityIcon;
            case AppMenu.MenuRegistro:
                return Icons.Material.Filled.Info;
            case AppMenu.MenuCatalog:
                return AppMenu.MenuCatalogIcon;
            case AppMenu.MenuOperacion:
                return AppMenu.MenuOperacionIcon;
            case AppMenu.MenuProceso:
                return AppMenu.MenuProcesoIcon;
            case AppMenu.MenuCustomers:
                return AppMenu.MenuCustomersIcon;
            case AppMenu.MenuAlerta:
                return Icons.Material.Filled.Warning;
            case AppMenu.MenuReporte:
                return AppMenu.MenuReporteIcon;
            case AppMenu.MenuDevelopment:
                return AppMenu.MenuDevelopmentIcon;
            case AppMenu.MenuHome:
                return Icons.Material.Filled.Home;
            case AppMenu.MenuOtrorgamiento:
                return AppMenu.MenuOtrorgamientoIcon;
            case AppMenu.MenuCobranza:
                return AppMenu.MenuCobranzaIcon;
            case AppMenu.MenuPortalCliente:
                return AppMenu.MenuPortalClienteIcon;
            case AppMenu.MenuTest:
                return AppMenu.MenuTestIcon;
            case AppMenu.MenuGeneral:
                return AppMenu.MenuGeneralIcon;
            case AppMenu.MenuAsignacion:
                return AppMenu.MenuAsignacionIcon;
            case AppMenu.MenuProject:
                return AppMenu.MenuProjectIcon;
            case AppMenu.MenuApoyo:
                return AppMenu.MenuApoyoIcon;
            default: return "";
        }
    }
}

public enum AccessPointType
{
    LeftMenu,
    Page,
    Element

}
