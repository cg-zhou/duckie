namespace Duckie.Shared.Utils.Localization;

public class EnAttribute : Attribute, ILocTextProvider
{
    public EnAttribute(string text)
    {
        Text = text;
    }

    public string CultureName { get; } = "en-US";
    public string Text { get; }
}