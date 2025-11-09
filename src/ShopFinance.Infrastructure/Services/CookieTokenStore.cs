//using BitzArt.Blazor.Cookies;
//using ShopFinance.Application.Common.Interfaces;

//namespace ShopFinance.Infrastructure.Services;

//public class CookieTokenStore : ITokenStore
//{
//    private readonly ICookieService _cookieService;
//    private const string CookieName = "authToken";

//    public CookieTokenStore(ICookieService cookieService)
//    {
//        _cookieService = cookieService;
//    }

//    public async Task SetAsync(string token)
//    {
//        await _cookieService.SetAsync(
//            key: CookieName,
//            value: token,
//            expiration: DateTimeOffset.Now.AddDays(7), // Expira en 7 días
//            httpOnly: true, // ✅ Importante para seguridad
//            secure: true,   // ✅ Solo sobre HTTPS
//            sameSiteMode: SameSiteMode.Strict // ✅ Protección CSRF
//        );
//    }

//    public async Task<string?> GetAsync()
//    {
//        var cookie = await _cookieService.GetAsync(CookieName);
//        if (cookie == null) return "";
//        return cookie.Value;
//    }

//    public async Task RemoveAsync()
//    {
//        await _cookieService.SetAsync(
//            key: CookieName,
//            value: "",
//            expiration: DateTimeOffset.Now.AddDays(-1), // ❌ Expira inmediatamente
//            httpOnly: true,
//            secure: true,
//            sameSiteMode: SameSiteMode.Strict
//        );
//    }
//}