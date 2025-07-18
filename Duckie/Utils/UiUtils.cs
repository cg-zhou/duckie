using System;
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

        public static void Info(string text, string caption = "Duckie")
        {
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void Warning(string text, string caption = "Warning")
        {
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void Error(string text, string caption = "Error")
        {
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void Error(Exception e, string text = "", string caption = "Error")
        {
            MessageBox.Show($"{text}{Environment.NewLine}Exception:{e.Message}{e.InnerException?.Message}", caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
