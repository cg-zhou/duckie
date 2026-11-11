using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Duckie.Utils.HotKeys;

public static partial class HotKeyManager
{
    public static class Interop
    {
        // WM_HOTKEY：系统通知窗口，已注册的全局快捷键被触发。
        public const int WmHotKey = 0x0312;

        // MOD_NOREPEAT：按住快捷键时不连续触发，松开后再次按下才触发。
        public const KeyModifiers ModNoRepeat = (KeyModifiers)0x4000;

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, KeyModifiers modifiers, Keys keys);

        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
    }
}
