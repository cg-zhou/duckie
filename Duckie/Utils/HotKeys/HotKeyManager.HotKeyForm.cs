using System.Windows.Forms;

namespace Duckie.Utils.HotKeys;

internal static partial class HotKeyManager
{
    public class HotKeyForm : Form
    {
        protected override void WndProc(ref Message m)
        {
            KeyPressed.Invoke(this, (int)m.WParam);
            base.WndProc(ref m);
        }

        public event EventHandler<int> KeyPressed;
    }
}
