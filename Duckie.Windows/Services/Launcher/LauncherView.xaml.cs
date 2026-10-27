using Duckie.Shared.Utils.Ui;
using System.Windows;
using System.Windows.Controls;

namespace Duckie.Windows.Services.Launcher;

/// <summary>
/// 快捷启动视图
/// </summary>
public partial class LauncherView : UserControl
{
    public LauncherView()
    {
        InitializeComponent();
        var config = Duckie.Shared.Services.UserConfigs.UserConfigService.Get().Launcher;
        LauncherEnabled.IsChecked = config?.Enabled == true;
        LaunchTarget.Text = config?.Target ?? "";
    }

    private void BrowseTarget_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new Microsoft.Win32.OpenFileDialog { Filter = "所有文件 (*.*)|*.*" };
        if (dialog.ShowDialog() == true) LaunchTarget.Text = dialog.FileName;
    }

    private void SaveLauncher_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            LauncherService.Save(new Duckie.Shared.Services.UserConfigs.LauncherConfig
            {
                Enabled = LauncherEnabled.IsChecked == true,
                Target = LaunchTarget.Text.Trim()
            });
            UiUtils.Info("启动项配置已保存，快捷键立即生效。", "快捷启动");
        }
        catch (Exception ex) { UiUtils.Warning(ex.Message, "保存失败"); }
    }

    private void RunLauncher_Click(object sender, RoutedEventArgs e)
    {
        try { LauncherService.Launch(); }
        catch (Exception ex) { UiUtils.Warning(ex.Message, "快捷启动失败"); }
    }
}
