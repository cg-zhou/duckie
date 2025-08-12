using Duckie.Shared.Utils.Localization;
using System.Windows.Forms;

namespace Duckie.Shared.Utils.Ui;

public static class UiUtils
{
    public static bool Confirm(string text, string caption)
    {
        var result = MessageBox.Show(text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        return result == DialogResult.Yes;
    }

    public static void Info(string text, string caption = null)
    {
        caption = caption ?? LocKey.AppTitle.Text();
        MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    public static void Warning(string text, string caption = null)
    {
        caption = caption ?? LocKey.Error_Warning.Text();
        MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }

    public static void Error(string text, string caption = null)
    {
        caption = caption ?? LocKey.Error_Error.Text();
        MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    public static void Error(Exception e, string text = "", string caption = null)
    {
        caption = caption ?? LocKey.Error_Error.Text();
        var message = string.IsNullOrEmpty(text)
            ? LocKey.Error_Exception.Text(e.Message + e.InnerException?.Message)
            : $"{text}{Environment.NewLine}{LocKey.Error_Exception.Text(e.Message + e.InnerException?.Message)}";
        MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    public static void BeginInvoke(Action action)
    {
        AppEnv.MainWindow.Dispatcher.BeginInvoke(action);
    }
}
