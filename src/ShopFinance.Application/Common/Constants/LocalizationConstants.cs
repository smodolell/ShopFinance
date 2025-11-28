namespace ShopFinance.Application.Common.Constants;

public static class LocalizationConstants
{
    public const string ResourcesPath = "Resources";
    /// <summary>
    /// Default language code. Set to Spanish (es-AR). 
    /// </summary>
    public const string DefaultLanguageCode = "es-AR";

    public static readonly LanguageCode[] SupportedLanguages =
    { 
        new()
        {
            Code = "es-AR",
            DisplayName = "Español (Argentino)"
        },
        new()
        {
            Code = "en-US",
            DisplayName = "English (United States)"
        }



    };
}

public class LanguageCode
{
    public string DisplayName { get; set; } = "es-AR";
    public string Code { get; set; } = "Spanish";
}
