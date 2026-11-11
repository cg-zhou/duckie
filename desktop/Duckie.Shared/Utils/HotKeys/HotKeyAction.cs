using System.Windows.Forms;

namespace Duckie.Utils.HotKeys;

public class HotKeyAction
{
    public HotKeyAction(string name, KeyModifiers modifiers, Keys keys, Action action, bool noRepeat = false)
    {
        Name = name;
        Modifiers = modifiers;
        Keys = keys;
        Action = action;
        NoRepeat = noRepeat;
    }

    public string Name { get; }
    public KeyModifiers Modifiers { get; }
    public Keys Keys { get; }
    public Action Action { get; }
    /// <summary>为 true 时，长按快捷键只触发一次，松开后可再次触发。</summary>
    public bool NoRepeat { get; }
}
