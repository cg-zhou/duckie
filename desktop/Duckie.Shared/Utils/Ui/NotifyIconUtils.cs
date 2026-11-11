using Duckie.Shared.Utils.Drawing;
using Duckie.Shared.Utils.Drawing.Ico;
using Duckie.Services.PacManager;
using Duckie.Shared.Utils.Localization;
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
            Text = $"{LocKey.AppTitle.Text()} — {LocKey.AppSubtitle.Text()}",
            ContextMenuStrip = new ContextMenuStrip()
        };

        notifyIcon.Click += NotifyIcon_Click;
        notifyIcon.ContextMenuStrip.Opening += (sender, args) => RefreshContextMenu();
        EmbeddedLocalizationManager.Instance.LanguageChanged += OnLanguageChanged;

        ResetIco();
    }

    public static void Release()
    {
        EmbeddedLocalizationManager.Instance.LanguageChanged -= OnLanguageChanged;
        SetIcon(null);
        notifyIcon?.Dispose();
        notifyIcon = null;
    }

    private static void OnLanguageChanged(object sender, EventArgs e)
    {
        UiUtils.BeginInvoke(() =>
        {
            if (notifyIcon == null) return;
            notifyIcon.Text = $"{LocKey.AppTitle.Text()} — {LocKey.AppSubtitle.Text()}";
            RefreshContextMenu();
        });
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

        items.Add(new ToolStripMenuItem(LocKey.AppTitle.Text(), null, (s, e) => UiUtils.BeginInvoke(() => (AppEnv.MainWindow as IMainWindow)?.ToggleWindow())));
        items.Add(new ToolStripSeparator());

        var pacMenuItem = new ToolStripMenuItem(LocKey.Tray_SetPac.Text());
        var currentPacUri = RegistryUtils.Pac.Get();
        pacMenuItem.DropDownItems.Add(new ToolStripMenuItem(LocKey.Tray_None.Text(), null, (s, e) => ApplyPac(null))
        {
            Checked = string.IsNullOrWhiteSpace(currentPacUri)
        });
        foreach (var pac in PacManagerService.GetAllPacConfigs())
        {
            var selectedPac = pac;
            pacMenuItem.DropDownItems.Add(new ToolStripMenuItem(selectedPac.Name, null, (s, e) => ApplyPac(selectedPac))
            {
                Checked = string.Equals(currentPacUri, selectedPac.Uri, StringComparison.OrdinalIgnoreCase)
            });
        }
        items.Add(pacMenuItem);

        var isChecked = RegistryUtils.StartUp.Get();
        items.Add(new ToolStripMenuItem(LocKey.Tray_LaunchOnStartup.Text(), null, (s, e) =>
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

        var languageMenuItem = new ToolStripMenuItem(LocKey.Tray_Language.Text());
        var currentLanguage = LocUtils.CurrentCulture.Name;
        languageMenuItem.DropDownItems.Add(new ToolStripMenuItem("简体中文", null, (s, e) =>
            UiUtils.BeginInvoke(() => EmbeddedLocalizationManager.Instance.SwitchLanguage("zh-CN")))
        {
            Checked = currentLanguage == "zh-CN"
        });
        languageMenuItem.DropDownItems.Add(new ToolStripMenuItem("English", null, (s, e) =>
            UiUtils.BeginInvoke(() => EmbeddedLocalizationManager.Instance.SwitchLanguage("en-US")))
        {
            Checked = currentLanguage == "en-US"
        });
        items.Add(languageMenuItem);

        items.Add(new ToolStripMenuItem(LocKey.Tray_Exit.Text(), null, (s, e) => Environment.Exit(0)));
    }

    private static void ApplyPac(Duckie.Shared.Services.UserConfigs.PacConfig pac)
    {
        if (!PacManagerService.ApplyPacConfig(pac))
        {
            UiUtils.Error(LocKey.Tray_FailedToApplyPac.Text());
        }
        RefreshContextMenu();
    }
}
