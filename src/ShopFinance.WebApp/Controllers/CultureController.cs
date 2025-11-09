using FluentValidation;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace ShopFinance.WebApp.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class CultureController : ControllerBase
{
    public IActionResult Set(string culture, string redirectUri)
    {
        if (culture != null)
        {
            HttpContext.Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(
                    new RequestCulture(culture, culture)));


            ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo(culture);
        }

        return LocalRedirect(redirectUri);
    }
}
