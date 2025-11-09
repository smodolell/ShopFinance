using Microsoft.AspNetCore.Components;
using BitzArt.Blazor.Cookies;
using MudBlazor;
using MudBlazor.Utilities;

namespace ShopFinance.WebApp.Components.Layout.States;

public class ThemeState
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ICookieService _cookieService;
    private readonly NavigationManager _navigationManager;

    public ThemeState(IHttpContextAccessor httpContextAccessor, ICookieService cookieService, NavigationManager navigationManager)
    {
        _httpContextAccessor = httpContextAccessor;
        _cookieService = cookieService;
        _navigationManager = navigationManager;

        InitializeTheme();
    }

    private void InitializeTheme()
    {
        var value = _httpContextAccessor.HttpContext?.Request.Cookies.Where(c => c.Key == "IsDark")
           .FirstOrDefault().Value;
        _isDark = string.IsNullOrEmpty(value) ? false : bool.Parse(value);

        var primaryColor = _httpContextAccessor.HttpContext?.Request.Cookies.Where(c => c.Key == "PrimaryColor")
            .FirstOrDefault().Value;
        _theme = new MudTheme()
        {
            PaletteLight = new PaletteLight()
            {
                Dark = "#141A21",
                AppbarBackground = "rgb(144,144,144)",
                AppbarText = "rgba(1,1,1, 0.70)",
                DrawerText = "rgba(1,1,1, 0.70)",
                DrawerBackground = "#ffffff",
            },
            PaletteDark = new PaletteDark()
            {
                Primary = "#007fff",
                Tertiary = "#594AE2",
                Black = "#27272f",
                Background = "#141A21",
                BackgroundGray = "#1C252E",
                Surface = "#1C252E",
                DrawerBackground = "#1C252E",
                DrawerText = "rgba(255,255,255, 0.50)",
                DrawerIcon = "rgba(255,255,255, 0.50)",
                AppbarBackground = "rgb(24,24,24)",
                AppbarText = "rgba(255,255,255, 0.70)",
                TextPrimary = "rgba(255,255,255, 0.70)",
                TextSecondary = "rgba(255,255,255, 0.50)",
                ActionDefault = "rgb(173, 173, 177)",
                TableLines = "rgba(255, 255, 255, 0.12)",
                TextDisabled = "rgba(0, 127, 255, 0.2)"
            },
            LayoutProperties = new LayoutProperties()
            {
                DefaultBorderRadius = "0px",
            }
        };

        // Si no hay color primario en la cookie, usar valor por defecto
        if (string.IsNullOrEmpty(primaryColor))
        {
            primaryColor = "#1668dc";
        }

        var color = new MudColor(primaryColor);
        UpdatePaletteColor(color);
    }


    private async Task SetCookieAsync(string key, string value, int days = 30)
    {
        try
        {
            await _cookieService.SetAsync(
                key: key,
                value: value,
                expiration: DateTimeOffset.Now.AddDays(days),
                httpOnly: false, // Permitir acceso desde JavaScript
                secure: false,   // Permitir HTTP (cambiar a true en producción con HTTPS)
                sameSiteMode: BitzArt.Blazor.Cookies.SameSiteMode.Lax
            );
        }
        catch (Exception ex)
        {
            // Log the error if needed
            Console.WriteLine($"Error setting cookie: {ex.Message}");
        }
    }

    private void UpdatePaletteColor(MudColor color)
    {
        _theme.PaletteLight.Primary = color;
        _theme.PaletteLight.PrimaryDarken = color.ColorRgbDarken().ToString(MudColorOutputFormats.RGB);
        _theme.PaletteLight.PrimaryLighten = color.ColorRgbLighten().ToString(MudColorOutputFormats.RGB);

        _theme.PaletteDark.Primary = color;
        _theme.PaletteDark.PrimaryDarken = color.ColorRgbDarken().ToString(MudColorOutputFormats.RGB);
        _theme.PaletteDark.PrimaryLighten = color.ColorRgbLighten().ToString(MudColorOutputFormats.RGB);
    }

    private bool _isDark;
    private MudTheme _theme = new();

    public event Action? ThemeChangeEvent;
    public event Action? IsDarkChangeEvent;

    public void LoadTheme()
    {
        IsDarkStateChanged();
        ThemeStateChanged();
    }

    public bool IsDark
    {
        get => _isDark;
        set
        {
            _isDark = value;
            _ = SetCookieAsync("IsDark", value.ToString());
            IsDarkStateChanged();
        }
    }

    public MudColor PrimaryColor
    {
        get => _theme.PaletteLight.Primary;
        set
        {
            UpdatePaletteColor(value);
            _ = SetCookieAsync("PrimaryColor", value.Value);
            ThemeStateChanged();
        }
    }

    public MudTheme MudTheme => _theme;

    private void ThemeStateChanged() => ThemeChangeEvent?.Invoke();
    private void IsDarkStateChanged() => IsDarkChangeEvent?.Invoke();
}