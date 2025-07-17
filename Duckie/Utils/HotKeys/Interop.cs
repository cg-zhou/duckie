using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Duckie.Utils.HotKeys
{
    internal static class Interop
    {
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, KeyModifiers modifiers, Keys keys);

        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
    }
}
