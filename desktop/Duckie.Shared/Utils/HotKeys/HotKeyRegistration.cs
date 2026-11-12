namespace Duckie.Utils.HotKeys;

internal sealed class HotKeyRegistration
{
    public int Id { get; set; }
    public HotKeyAction Action { get; set; }
    public HotKeyBinding Binding { get; set; }
}
