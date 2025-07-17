using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Duckie.Utils
{
    internal class NotifyIconUtils
    {
        private static NotifyIcon notifyIcon;

        public static void Initialize()
        {
            notifyIcon = new NotifyIcon
            {
                Visible = true,
                ContextMenuStrip = new ContextMenuStrip()
            };

            notifyIcon.Click += NotifyIcon_Click;

            if (AppUtils.TryGetResourceStream("duck.ico", out var stream))
            {
                SetIcon(stream);
            }
        }

        private static void SetIcon(Stream stream)
        {
            if (notifyIcon.Icon != null)
            {
                notifyIcon.Icon.Dispose();
            }

            var icon = new Icon(stream);
            notifyIcon.Icon = icon;
        }

        private static void NotifyIcon_Click(object sender, EventArgs e)
        {
            if (e is MouseEventArgs mouseEventArgs)
            {
                if (mouseEventArgs.Button == MouseButtons.Left)
                {
                    App.MainWindow.Toggle();
                }
            }
        }

        public static void Notify(string text, string caption)
        {
            notifyIcon.ShowBalloonTip(3000, caption, text, ToolTipIcon.None);
        }
    }
}
