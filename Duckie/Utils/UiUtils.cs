using System.Windows.Forms;

namespace Duckie.Utils
{
    internal static class UiUtils
    {
        public static bool Confirm(string text, string caption)
        {
            var result = MessageBox.Show(text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return result == DialogResult.Yes;
        }

        public static void Warning(string text, string caption)
        {
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
