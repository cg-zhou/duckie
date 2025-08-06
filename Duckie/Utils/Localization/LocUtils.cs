using System.Globalization;

namespace Duckie.Utils.Localization;

public static class LocUtils
{
    public static void Initialize()
    {
        EmbeddedLocalizationManager.Instance.LoadSavedLanguage();
    }

    public static string GetString(string key, params object[] args)
    {
        return EmbeddedLocalizationManager.Instance.GetString(key, args);
    }

    public static CultureInfo CurrentCulture => EmbeddedLocalizationManager.Instance.CurrentCulture;
}
