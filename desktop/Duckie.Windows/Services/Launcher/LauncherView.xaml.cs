using Duckie.Shared.Services.UserConfigs;
using Duckie.Shared.Utils.Localization;
using Duckie.Shared.Views.Common;
using Duckie.Utils.HotKeys;
using System.Windows;
using System.Windows.Controls;

namespace Duckie.Windows.Services.Launcher;

public class LauncherRow
{
    public LauncherConfig Config { get; set; }
    public string Name => Config.Name;
    public string Target => Config.Target;
    public HotKeyAction Action { get; set; }
}

public partial class LauncherView : UserControl
{
    public LauncherView() { InitializeComponent(); Refresh(); }

    private void Refresh() => Items.ItemsSource = (UserConfigService.Get().Launchers ?? [])
        .Select(x => new LauncherRow { Config = x, Action = LauncherService.CreateAction(x) }).ToArray();

    private void Add_Click(object sender, RoutedEventArgs e) => Edit(null);
    private void Edit_Click(object sender, RoutedEventArgs e) => Edit(((LauncherRow)((Button)sender).Tag).Config);
    private void Run_Click(object sender, RoutedEventArgs e)
    {
        try { LauncherService.Launch(((LauncherRow)((Button)sender).Tag).Config.Id); }
        catch (Exception ex) { MessageBox.Show(ex.Message, LocKey.Launcher_LaunchFailed.Text()); }
    }
    private void Delete_Click(object sender, RoutedEventArgs e)
    {
        var row = (LauncherRow)((Button)sender).Tag;
        if (MessageBox.Show(LocKey.Launcher_ConfirmDelete.Text(row.Name), LocKey.Launcher_DeleteTitle.Text(), MessageBoxButton.YesNo) != MessageBoxResult.Yes) return;
        try
        {
            var config = UserConfigService.Get();
            config.Launchers = (config.Launchers ?? []).Where(x => x.Id != row.Config.Id).ToArray();
            config.HotKeys = (config.HotKeys ?? []).Where(x => x.ActionId != row.Action.Id).ToArray();
            UserConfigService.Set(config);
            HotKeyManager.Unregister(row.Action);
            HotKeyBindings.NotifyChanged();
            Refresh();
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, LocKey.Launcher_DeleteFailed.Text()); }
    }
    private void Edit(LauncherConfig existing)
    {
        var content = new LauncherEditContent(existing);
        var dialog = DialogEx.Create(existing == null ? LocKey.Launcher_AddTitle.Text() : LocKey.Launcher_EditTitle.Text(), content, DialogButtons.OKCancel, d =>
        {
            if (!content.ValidateAndCreateConfig()) return;
            try
            {
                var config = UserConfigService.Get();
                var item = content.LauncherConfig;
                config.Launchers = existing == null ? (config.Launchers ?? []).Append(item).ToArray()
                    : (config.Launchers ?? []).Select(x => x.Id == item.Id ? item : x).ToArray();
                UserConfigService.Set(config); d.DialogResult = true; d.Close(); Refresh();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, LocKey.Launcher_SaveFailed.Text()); }
        });
        dialog.Width = 440;
        dialog.SizeToContent = SizeToContent.Height;
        dialog.SetResourceReference(FontFamilyProperty, "DesignFontFamily");
        dialog.SetResourceReference(BackgroundProperty, "DesignSurfaceBrush");
        dialog.Resources["PrimaryButtonStyle"] = FindResource("DesignPrimaryButton");
        dialog.Resources["ModernButtonStyle"] = FindResource("DesignButton");
        dialog.ShowDialog();
    }
}
