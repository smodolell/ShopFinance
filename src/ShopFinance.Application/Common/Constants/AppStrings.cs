using System.Globalization;
using System.Resources;

namespace ShopFinance.Application.Common.Constants;

public static class AppStrings
{
    public const string APPSTRINGS_RESOURCE_ID = "ShopFinance.Application.Common.Resources.Constants.AppStrings";

    private static readonly ResourceManager rm;

    static AppStrings()
    {
        rm = new ResourceManager(APPSTRINGS_RESOURCE_ID, typeof(AppStrings).Assembly);
    }

    public static string Refresh => Localize("Refresh");
    public static string New => Localize("New");
    public static string Edit => Localize("Edit");
    public static string Create => Localize("Create");
    public static string Save => Localize("Save");
    public static string Delete => Localize("Delete");
    public static string More => Localize("More");
    public static string Print => Localize("Print");


    private static string Localize(string key)
    {
        try
        {
            // Try to get localized string using current UI culture
            var localizedString = rm.GetString(key, CultureInfo.CurrentUICulture);

            // If localized string is found and not empty, return it
            if (!string.IsNullOrEmpty(localizedString))
            {
                return localizedString;
            }

            // If not found in current culture, try using invariant culture as fallback
            localizedString = rm.GetString(key, CultureInfo.InvariantCulture);
            if (!string.IsNullOrEmpty(localizedString))
            {
                return localizedString;
            }
        }
        catch (Exception)
        {
            // If an exception occurs, fallback to returning the key
            // This may happen when resource files are missing or corrupted
        }

        // Final fallback: return the key itself
        return key;
    }
}
