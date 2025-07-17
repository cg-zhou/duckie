using System;
using System.Windows.Forms;

namespace Duckie.Utils.HotKeys
{
    internal class Item
    {
        public int Id { get; set; }
        public KeyModifiers Modifiers { get; set; }
        public Keys Keys { get; set; }
        public Action Action { get; set; }
    }
}
