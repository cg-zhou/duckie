namespace Duckie.Utils.Localization;

public static class LocKeyExtension
{
    public static string Text(this LocKey locKey, params object[] args)
    {
        return LocUtils.GetString(locKey, args);
    }
}
