using Microsoft.AspNetCore.Components;
using ShopFinance.WebApp.Services;
using ShopFinance.WebApp.Services.Dtos;

namespace ShopFinance.WebApp.Components.Layout.NavMenus;

public partial class NavMenu
{
    [Inject]
    public ILayoutService LayoutService { get; set; } = null!;

    public HashSet<AccessPointDto>? NavMenuItems { get; set; }

    protected override void OnInitialized()
    {
        _layoutState.NavIsOpenEvent += () => StateHasChanged();
        _themeState.IsDarkChangeEvent += OnThemeChanged;
    }
    private void OnThemeChanged()
    {
        InvokeAsync(StateHasChanged);
    }
    private async Task<HashSet<AccessPointDto>> InitMenu()
    {
        var menus = await LayoutService.GetMenu();

        return menus;

    }
    private void NavTo(AccessPointDto item)
    {
        _layoutState.NavTo(item);
    }
    protected async override Task OnAfterRenderAsync(bool firstRender)  
    {
        if (firstRender)
        {
            NavMenuItems = await InitMenu();
            StateHasChanged();
        }
    }

    public void Dispose()
    {
        _themeState.IsDarkChangeEvent -= OnThemeChanged;
    }
}
