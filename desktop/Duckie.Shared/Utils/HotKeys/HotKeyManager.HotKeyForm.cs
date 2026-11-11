using System.Windows.Forms;

namespace Duckie.Utils.HotKeys;

public static partial class HotKeyManager
{
    public class HotKeyForm : Form
    {
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Interop.WmHotKey)
                KeyPressed?.Invoke(this, (int)m.WParam);
            base.WndProc(ref m);
        }

        public event EventHandler<int> KeyPressed;
    }
}
