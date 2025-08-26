using Duckie.Shared.Utils.Drawing;
using Duckie.Shared.Utils.Drawing.Ico;
using Duckie.Shared.Views;
using Duckie.Utils.Registry;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Duckie.Shared.Utils.Ui;

public class NotifyIconUtils
{
    private const string icoEmbeddedResourceName = "duck.ico";
    private static NotifyIcon notifyIcon;

    public static void Initialize()
    {
        notifyIcon = new NotifyIcon
        {
            Visible = true,
            ContextMenuStrip = new ContextMenuStrip()
        };

        notifyIcon.Click += NotifyIcon_Click;

        ResetIco();
    }

    public static void Release()
    {
        SetIcon(null);
    }

    public static void SetIcoBage(Color color)
    {
        if (AppUtils.TryGetEmbeddedResource(icoEmbeddedResourceName, out var stream))
        {
            var images = IcoUtils.ParseIco(stream);
            var image = images.Select(x => x.Value).OrderByDescending(x => x.Width).FirstOrDefault();
            LogoUtils.DrawBadge(image, color);

            var icon = image.ToIco();
            SetIcon(icon.ToStream());
        }
    }

    public static void ResetIco()
    {
        if (AppUtils.TryGetEmbeddedResource(icoEmbeddedResourceName, out var stream))
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

        if (stream != null)
        {
            var icon = new Icon(stream);
            notifyIcon.Icon = icon;
        }
        else
        {
            notifyIcon.Icon = null;
        }
    }

    private static void NotifyIcon_Click(object sender, EventArgs e)
    {
        if (e is MouseEventArgs mouseEventArgs)
        {
            if (mouseEventArgs.Button == MouseButtons.Left)
            {
                (AppEnv.MainWindow as IMainWindow).ToggleWindow();
            }
        }
    }


    public static void Notify(string text, string caption)
    {
        notifyIcon.ShowBalloonTip(3000, caption, text, ToolTipIcon.None);
    }

    public static void RefreshContextMenu()
    {
        var items = notifyIcon.ContextMenuStrip.Items;
        items.Clear();

        items.Add(new ToolStripMenuItem("Duckie", null, (s, e) => UiUtils.BeginInvoke(() => (AppEnv.MainWindow as IMainWindow)?.ToggleWindow())));
        items.Add(new ToolStripSeparator());

        var pacMenuItem = new ToolStripMenuItem("Set PAC");
        pacMenuItem.DropDownItems.Add(new ToolStripMenuItem("None", null, (s, e) => { }));
        pacMenuItem.DropDownItems.Add(new ToolStripMenuItem("Test2", null, (s, e) => { }) { Checked = true });
        items.Add(pacMenuItem);

        var isChecked = RegistryUtils.StartUp.Get();
        items.Add(new ToolStripMenuItem("Launch On Startup", null, (s, e) =>
        {
            var currentChecked = RegistryUtils.StartUp.Get();
            var newChecked = !currentChecked;

            // 直接更新注册表，注册表是唯一数据源
            RegistryUtils.StartUp.Set(newChecked);

            RefreshContextMenu();
        })
        {
            Checked = isChecked
        });

        items.Add(new ToolStripMenuItem("Exit", null, (s, e) => Environment.Exit(0)));
    }
}
