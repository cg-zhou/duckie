using System;
using System.Windows.Forms;

namespace Duckie.Utils.HotKeys
{
    public class HotKeyAction
    {
        public HotKeyAction(string name, KeyModifiers modifiers, Keys keys, Action action)
        {
            Name = name;
            Modifiers = modifiers;
            Keys = keys;
            Action = action;
        }

        public string Name { get; }
        public KeyModifiers Modifiers { get; }
        public Keys Keys { get; }
        public Action Action { get; }
    }
}
