using Duckie.Utils.Localization;
using System;
using System.Windows.Forms;

namespace Duckie.Utils.Ui
{
    internal static class UiUtils
    {
        public static bool Confirm(string text, string caption)
        {
            var result = MessageBox.Show(text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return result == DialogResult.Yes;
        }

        public static void Info(string text, string caption = null)
        {
            caption = caption ?? LocUtils.GetString("AppTitle");
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void Warning(string text, string caption = null)
        {
            caption = caption ?? LocUtils.GetString("Error_Warning");
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void Error(string text, string caption = null)
        {
            caption = caption ?? LocUtils.GetString("Error_Error");
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void Error(Exception e, string text = "", string caption = null)
        {
            caption = caption ?? LocUtils.GetString("Error_Error");
            var message = string.IsNullOrEmpty(text)
                ? LocUtils.GetString("Error_Exception", e.Message + e.InnerException?.Message)
                : $"{text}{Environment.NewLine}{LocUtils.GetString("Error_Exception", e.Message + e.InnerException?.Message)}";
            MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void BeginInvoke(Action action)
        {
            App.MainWindow.Dispatcher.BeginInvoke(action);
        }
    }
}
