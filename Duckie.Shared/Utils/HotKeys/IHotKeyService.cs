namespace Duckie.Utils.HotKeys;

public interface IHotKeyService
{
    IEnumerable<HotKeyAction> Register();
}
