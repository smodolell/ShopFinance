//using Microsoft.AspNetCore.Components.Authorization;
//using Microsoft.AspNetCore.Http;
//using ShopFinance.Application.Common.Interfaces;
//using System.Security.Claims;

//namespace ShopFinance.Infrastructure.Authentication;

//public class JwtAuthStateProvider : AuthenticationStateProvider, IAuthStateService
//{
//    private readonly ITokenStore _tokenStore;
//    private readonly IJwtHelper _jwtHelper;
//    private readonly IHttpContextAccessor _httpContextAccessor;

//    public event Action<ClaimsPrincipal>? UserChanged;

//    public JwtAuthStateProvider(
//        ITokenStore tokenStore,
//        IJwtHelper jwtHelper,
//        IHttpContextAccessor httpContextAccessor = null)
//    {
//        _tokenStore = tokenStore;
//        _jwtHelper = jwtHelper;
//        _httpContextAccessor = httpContextAccessor;
//    }

//    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
//    {
//        var token = await _tokenStore.GetAsync();

//        //// Si no hay token, intentar desde cookies HTTP (para SSR)
//        //if (string.IsNullOrEmpty(token) && _httpContextAccessor?.HttpContext != null)
//        //{
//        //    token = _httpContextAccessor.HttpContext.Request.Cookies["UserToken"];
//        //    if (!string.IsNullOrEmpty(token))
//        //    {
//        //        await _tokenStore.SetAsync(token);
//        //    }
//        //}

//        if (string.IsNullOrWhiteSpace(token))
//            return CreateUnauthenticatedState();

//        var principal = await _jwtHelper.ValidToken(token); 
//        if (principal == null)
//        {
//            await _tokenStore.RemoveAsync();
//            return CreateUnauthenticatedState();
//        }

//        return new AuthenticationState(principal); // ← Ahora principal es ClaimsPrincipal, no Task<ClaimsPrincipal?>
//    }

//    public async Task SignInAsync(string token)
//    {
//        //await _tokenStore.SetAsync(token);

//        // También guardar en cookie para compatibilidad SSR
//        //if (_httpContextAccessor?.HttpContext != null)
//        //{
//        //    _httpContextAccessor.HttpContext.Response.Cookies.Append("UserToken", token, new CookieOptions
//        //    {
//        //        HttpOnly = true,
//        //        Secure = true,
//        //        SameSite = SameSiteMode.Strict,
//        //        Expires = DateTimeOffset.Now.AddDays(7)
//        //    });
//        //}

//        var principal = await _jwtHelper.ValidToken(token); // ← AGREGAR AWAIT
//        if (principal != null)
//        {
//            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
//            UserChanged?.Invoke(principal); // ← Ahora principal es ClaimsPrincipal
//        }
//    }

//    public async Task SignOutAsync()
//    {
//        await _tokenStore.RemoveAsync();

//        // Limpiar cookie también
//        if (_httpContextAccessor?.HttpContext != null)
//        {
//            _httpContextAccessor.HttpContext.Response.Cookies.Delete("UserToken");
//        }

//        var unauthState = CreateUnauthenticatedState();
//        NotifyAuthenticationStateChanged(Task.FromResult(unauthState));
//        UserChanged?.Invoke(new ClaimsPrincipal(new ClaimsIdentity()));
//    }

//    private static AuthenticationState CreateUnauthenticatedState()
//    {
//        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
//    }

//    // Método para forzar actualización del estado
//    public async Task RefreshAuthenticationStateAsync()
//    {
//        var authState = await GetAuthenticationStateAsync();
//        NotifyAuthenticationStateChanged(Task.FromResult(authState));
//    }
//}