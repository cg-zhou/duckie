using System.Windows.Forms;

namespace Duckie.Utils.HotKeys;

public sealed class HotKeyBinding : IEquatable<HotKeyBinding>
{
    public KeyModifiers Modifiers { get; set; }
    public Keys Key { get; set; }
    public HotKeyBinding() { }
    public HotKeyBinding(KeyModifiers modifiers, Keys key)
    {
        Modifiers = key == Keys.None ? KeyModifiers.None : modifiers;
        Key = key;
    }
    public bool IsEmpty => Key == Keys.None;
    public bool Equals(HotKeyBinding other) => other != null && Modifiers == other.Modifiers && Key == other.Key;
    public override bool Equals(object obj) => Equals(obj as HotKeyBinding);
    public override int GetHashCode() => ((int)Modifiers * 397) ^ (int)Key;
    public override string ToString()
    {
        if (IsEmpty) return "";
        var parts = new List<string>();
        if (Modifiers.HasFlag(KeyModifiers.Control)) parts.Add("Ctrl");
        if (Modifiers.HasFlag(KeyModifiers.Alt)) parts.Add("Alt");
        if (Modifiers.HasFlag(KeyModifiers.Shift)) parts.Add("Shift");
        if (Modifiers.HasFlag(KeyModifiers.Win)) parts.Add("Win");
        parts.Add(Key >= Keys.D0 && Key <= Keys.D9 ? ((int)Key - (int)Keys.D0).ToString()
            : Key == Keys.Oemtilde ? "`" : Key == Keys.Oemplus ? "+"
            : Key == Keys.OemMinus ? "-" : Key == Keys.LaunchApplication2 ? "Application 2" : Key.ToString());
        return string.Join(" + ", parts);
    }
}
