using Duckie.Shared.Services.UserConfigs;
using Duckie.Shared.Utils.Localization;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Duckie.Windows.Services.Launcher;

public partial class LauncherEditContent : UserControl
{
    private readonly string id;
    public LauncherConfig LauncherConfig { get; private set; }

    public LauncherEditContent(LauncherConfig config = null)
    {
        InitializeComponent();
        id = config?.Id ?? Guid.NewGuid().ToString("N");
        NameTextBox.Text = config?.Name ?? "";
        TargetTextBox.Text = config?.Target ?? "";
        NameTextBox.Focus();
    }

    private void Browse_Click(object sender, RoutedEventArgs e)
    {
        var picker = new Microsoft.Win32.OpenFileDialog { Filter = LocKey.Launcher_FileFilter.Text() };
        if (picker.ShowDialog() == true) TargetTextBox.Text = picker.FileName;
    }

    public bool ValidateAndCreateConfig()
    {
        var name = NameTextBox.Text.Trim();
        var path = TargetTextBox.Text.Trim();
        if (string.IsNullOrWhiteSpace(name) || !Path.IsPathRooted(path) || !File.Exists(Environment.ExpandEnvironmentVariables(path)))
        {
            MessageBox.Show(LocKey.Launcher_InvalidInput.Text());
            return false;
        }
        LauncherConfig = new LauncherConfig { Id = id, Name = name, Target = path };
        return true;
    }
}
