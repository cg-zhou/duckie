namespace Duckie.Utils.Registry;

public static partial class RegistryUtils
{
    public static void SetCurrentUserValue(string key, string name, string value)
    {
        using (var registry = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(key, true))
        {
            registry?.SetValue(name, value);
        }
    }

    public static string GetCurrentUserValue(string key, string name)
    {
        using (var registry = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(key, false))
        {
            return registry?.GetValue(name)?.ToString() ?? string.Empty;
        }
    }
}
