namespace Duckie.Utils.Localization;

public class CnAttribute : Attribute, ILocTextProvider
{
    public CnAttribute(string text)
    {
        Text = text;
    }
    public string CultureName { get; } = "zh-CN";
    public string Text { get; }
}
