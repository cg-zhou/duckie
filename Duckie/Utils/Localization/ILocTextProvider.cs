namespace Duckie.Utils.Localization;

public interface ILocTextProvider
{
    public string Text { get; }
    public string CultureName { get; }
}
