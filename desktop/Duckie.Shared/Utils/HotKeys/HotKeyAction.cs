using Duckie.Shared.Utils.Localization;
using System.Windows.Forms;

namespace Duckie.Utils.HotKeys;

public class HotKeyAction
{
    public HotKeyAction(string id, LocKey nameKey, KeyModifiers modifiers, Keys key, Action action, bool noRepeat = false)
        : this(id, nameKey, new HotKeyBinding(modifiers, key), action, noRepeat) { }

    public HotKeyAction(string id, LocKey nameKey, HotKeyBinding defaultBinding, Action action, bool noRepeat = false)
    {
        NameKey = nameKey;
        Id = id;
        DefaultBinding = defaultBinding;
        Action = action;
        NoRepeat = noRepeat;
    }

    public LocKey NameKey { get; }
    public string Name => NameKey.Text();
    public string Id { get; }
    public HotKeyBinding DefaultBinding { get; }
    public Action Action { get; }
    /// <summary>为 true 时，长按快捷键只触发一次，松开后可再次触发。</summary>
    public bool NoRepeat { get; }
}
